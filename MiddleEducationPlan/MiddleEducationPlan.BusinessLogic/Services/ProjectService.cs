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
        public const string ENTITY_NAME = "Project";
        private readonly ITaskService taskService;
        private readonly IStorageAccountService<ProjectEntity> storageAccountService;

        public ProjectService(ITaskService taskService, IStorageAccountService<ProjectEntity> storageAccountService)
        {
            this.taskService = taskService;
            this.storageAccountService = storageAccountService;
        }

        public async Task<TableResult> AddProjectAsync(AddProjectModel project)
        {
            Guid id = Guid.NewGuid();
            return await this.storageAccountService.AddEntityAsync(new ProjectEntity 
            { 
                Id = id,
                PartitionKey = id.ToString(),
                RowKey = id.ToString(),
                Name = project.Name,
                ProjectTypeInteger = (int)(project.ProjectType ?? ProjectType.None),
                Description = project.Description
            });
        }

        public async Task<TableResult> UpdateProjectAsync(Guid id, UpdateProjectModel project)
        {
            var projectEntity = await this.storageAccountService.GetEntityByIdAsync(id);

            if (projectEntity == null)
                return null;

            projectEntity.Name = project.Name;
            projectEntity.ProjectTypeInteger = (int)project.ProjectType;
            projectEntity.Description = project.Description;

            return await this.storageAccountService.UpdateEntityAsync(projectEntity);
        }

        public async Task<ProjectEntity> GetProjectByIdAsync(Guid id)
        {
            var project = await this.storageAccountService.GetEntityByIdAsync(id);
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

            var projects = await this.storageAccountService.ExecuteQuerySegmentedAsync(query);

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
            string nameFilter = "";

            if (filter.Name != null)
            {
                nameFilter = TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, filter.Name);
            }

            if (filter.Name != null)
                return nameFilter;
            return null;
        }

        public async Task<TableResult> DeleteProjectByIdAsync(Guid id)
        {
            return await this.storageAccountService.DeleteEntityByIdAsync(id);
        }
    }
}
