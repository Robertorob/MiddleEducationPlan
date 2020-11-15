using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Common.Services
{
    public interface IStorageAccountService<TEntity> where TEntity : TableEntity, new()
    {
        Task<TableResult> AddEntityAsync(TableEntity entity);
        Task<TableResult> DeleteEntityByIdAsync(Guid id);
        Task<TEntity> GetEntityByIdAsync(Guid id);
        Task<TableResult> UpdateEntityAsync(TableEntity entity);

        Task<TableQuerySegment<TEntity>> ExecuteQuerySegmentedAsync(TableQuery<TEntity> query);
    }
}