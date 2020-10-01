using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.Models.Task;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.BusinessLogic.Services
{
    public class ProjectService : IProjectService
    {
        private const string ENTITY_NAME = "Project";
        private readonly CloudTable cloudTable;
        private readonly ITaskService taskService;
        private readonly StorageAccountService<ProjectEntity> storageAccountService;

        public ProjectService(ITaskService taskService, ICloudTableClientFactory cloudTableClientFactory)
        {
            var cloudTableClient = cloudTableClientFactory.GetCloudTableClient();
            this.cloudTable = cloudTableClient.GetTableReference(ProjectService.ENTITY_NAME);
            this.storageAccountService = new StorageAccountService<ProjectEntity>(this.cloudTable);
            this.taskService = taskService;
        }

        public async Task<TableResult> AddProjectAsync(AddProjectModel project)
        {
            await this.cloudTable.CreateIfNotExistsAsync();

            Guid id = Guid.NewGuid();
            int code = await GetProjectCodeAndIncrementAsync(this.cloudTable);
            return await this.storageAccountService.AddEntityAsync(new ProjectEntity 
            { 
                Id = id,
                Code = code,
                PartitionKey = id.ToString(),
                RowKey = id.ToString(),
                Name = project.Name,
                ProjectTypeInteger = (int)(project.ProjectType ?? ProjectType.None),
                Description = project.Description,
                Owners = string.Join(';', project.Owners)
            });
        }

        public async Task<TableResult> UpdateProjectAsync(Guid id, UpdateProjectModel project)
        {
            var projectEntity = await this.storageAccountService.GetEntityById(id);

            if (projectEntity == null)
                return null;

            projectEntity.Name = project.Name;
            projectEntity.ProjectTypeInteger = (int)project.ProjectType;
            projectEntity.Description = project.Description;
            if(projectEntity.Owners.Any())
                projectEntity.Owners = string.Join(';', project.Owners);

            return await this.storageAccountService.UpdateEntityAsync(projectEntity);
        }

        public async Task<ProjectEntity> GetProjectByIdAsync(Guid id)
        {
            var project = await this.storageAccountService.GetEntityById(id);
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

            var projects = await this.cloudTable.ExecuteQuerySegmentedAsync(query, null);

            if (filter.IncludeTasks)
            {
                foreach (var project in projects)
                {
                    project.Tasks = await this.taskService.GetTasksAsync(new GetTaskModel { ProjectId = project.Id });
                } 
            }

            return projects.Where(f => f.RowKey != "0").ToList();
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
            return await this.storageAccountService.DeleteEntityByIdAsync(id);
        }
    }
}
