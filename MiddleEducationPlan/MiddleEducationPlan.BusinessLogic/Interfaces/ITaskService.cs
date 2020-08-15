using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.Models.Task;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiddleEducationPlan.BusinessLogic.Interfaces
{
    public interface ITaskService
    {
        public Task<TableResult> AddTaskAsync(AddTaskModel task);

        public Task<TableResult> UpdateTaskAsync(Guid id, UpdateTaskModel task);

        public Task<TaskEntity> GetTaskByIdAsync(Guid id);

        public Task<List<TaskEntity>> GetTasksAsync(GetTaskModel filter);

        public Task<TableResult> DeleteTaskByIdAsync(Guid id);
    }
}
