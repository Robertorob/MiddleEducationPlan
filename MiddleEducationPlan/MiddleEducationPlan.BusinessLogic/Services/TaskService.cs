using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.Models.Task;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Services
{
    public class TaskService : StorageAccountService<TaskEntity>
    {
        private const string ENTITY_NAME = "Task";
        private readonly CloudTable table;

        public TaskService(IConfiguration configuration, AzureKeyVaultService keyVaultServie) : base(configuration, keyVaultServie) 
        { 
            this.table = this.tableClient.GetTableReference(TaskService.ENTITY_NAME);
        }

        public async Task<TableResult> AddTaskAsync(AddTaskModel task)
        {
            Guid id = Guid.NewGuid();
            return await AddEntityAsync(new TaskEntity 
            { 
                Id = id,
                PartitionKey = task.ProjectId.ToString(),
                RowKey = id.ToString(),
                Name = task.Name,
                Description = task.Description,
                ProjectId = task.ProjectId ?? new Guid()
            }, this.table);
        }

        public async Task<TableResult> UpdateTaskAsync(Guid id, UpdateTaskModel task)
        {
            var taskEntity = await this.GetEntityById(id, this.table);

            if (taskEntity == null)
                return null;

            taskEntity.Name = task.Name;
            taskEntity.Description = task.Description;
            taskEntity.ProjectId = task.ProjectId ?? new Guid();

            return await this.UpdateEntityAsync(taskEntity, this.table);
        }

        public async Task<TaskEntity> GetTaskByIdAsync(Guid id)
        {
            return await GetEntityById(id, this.table);
        }

        public async Task<List<TaskEntity>> GetTasksAsync(GetTaskModel filter)
        {
            var query = new TableQuery<TaskEntity>();

            string combined = CombineTaskFilters(filter);

            if (!string.IsNullOrEmpty(combined))
                query = query.Where(combined);

            var tasks = await table.ExecuteQuerySegmentedAsync(query, null);

            return tasks.ToList();
        }

        private string CombineTaskFilters(GetTaskModel filter)
        {
            string nameFilter = "";
            string projectIdFilter = "";
            if (filter.ProjectId != null)
            {
                projectIdFilter = TableQuery.GenerateFilterConditionForGuid("ProjectId", QueryComparisons.Equal, filter.ProjectId ?? new Guid());
            }
            if (filter.Name != null)
            {
                nameFilter = TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, filter.Name);
            }

            if (filter.ProjectId != null & filter.Name != null)
            {
                return TableQuery.CombineFilters(projectIdFilter, TableOperators.And, nameFilter);
            }

            if (filter.ProjectId != null)
                return projectIdFilter;
            if (filter.Name != null)
                return nameFilter;
            return null;
        }

        public async Task<TableResult> DeleteTaskByIdAsync(Guid id)
        {
            return await DeleteEntityByIdAsync(id, this.table);
        }
    }
}
