using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.Models.Task;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Services
{
    public class ProjectService : StorageAccountService<ProjectEntity>
    {
        private const string ENTITY_NAME = "Project";
        private readonly CloudTable table;
        private readonly TaskService taskService;

        public ProjectService(IAzureKeyVaultService keyVaultServie, TaskService taskService) : base(keyVaultServie) 
        { 
            this.table = this.tableClient.GetTableReference(ProjectService.ENTITY_NAME);
            this.taskService = taskService;
        }

        public async Task<TableResult> AddProjectAsync(AddProjectModel project)
        {
            Guid id = Guid.NewGuid();
            int code = await GetProjectCodeAndIncrementAsync(table);
            return await AddEntityAsync(new ProjectEntity 
            { 
                Id = id,
                Code = code,
                PartitionKey = id.ToString(),
                RowKey = id.ToString(),
                Name = project.Name
            }, this.table);
        }

        public async Task<TableResult> UpdateProjectAsync(Guid id, UpdateProjectModel project)
        {
            var projectEntity = await this.GetEntityById(id, this.table);

            if (projectEntity == null)
                return null;

            projectEntity.Name = project.Name;

            return await this.UpdateEntityAsync(projectEntity, this.table);
        }

        public async Task<ProjectEntity> GetProjectByIdAsync(Guid id)
        {
            var project = await GetEntityById(id, this.table);
            var tasks = await this.taskService.GetTasksAsync(new GetTaskModel
            {
                ProjectId = id
            });

            if(project != null)
                project.Tasks = tasks;

            return project;
        }

        public async Task<List<ProjectEntity>> GetProjectsAsync(GetProjectModel filter)
        {
            var query = new TableQuery<ProjectEntity>();
            string combined = CombineProjectFilters(filter);

            if (!string.IsNullOrEmpty(combined))
                query = query.Where(combined);

            var projects = await table.ExecuteQuerySegmentedAsync(query, null);

            if (filter.IncludeTasks)
            {
                foreach (var project in projects)
                {
                    project.Tasks = await this.taskService.GetTasksAsync(new GetTaskModel { ProjectId = project.Id });
                } 
            }

            return projects.ToList();
        }

        private string CombineProjectFilters(GetProjectModel filter)
        {
            string codeFilter = "";
            string nameFilter = "";
            if (filter.Code != null)
            {
                codeFilter = TableQuery.GenerateFilterConditionForInt("Code", QueryComparisons.Equal, filter.Code ?? 0);
            }
            if (filter.Name != null)
            {
                nameFilter = TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, filter.Name);
            }

            if (filter.Code != null & filter.Name != null)
            {
                return TableQuery.CombineFilters(codeFilter, TableOperators.And, nameFilter);
            }

            if (filter.Code != null)
                return codeFilter;
            if (filter.Name != null)
                return nameFilter;
            return null;
        }

        private async Task<int> GetProjectCodeAndIncrementAsync(CloudTable table)
        {
            var query = new TableQuery<ProjectEntity>()
                    .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "0"));
            var idEntity = (await table.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();
            idEntity.Code++;
            var incrementCode = TableOperation.InsertOrReplace(idEntity);

            await table.ExecuteAsync(incrementCode);

            return idEntity.Code;
        }

        public async Task<TableResult> DeleteProjectByIdAsync(Guid id)
        {
            return await DeleteEntityByIdAsync(id, this.table);
        }
    }
}
