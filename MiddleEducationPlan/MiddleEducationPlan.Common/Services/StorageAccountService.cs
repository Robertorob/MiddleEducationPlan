using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Common.Services
{
    public class StorageAccountService<TEntity> : IStorageAccountService<TEntity> where TEntity : TableEntity, new()
    {
        protected readonly CloudTable cloudTable;

        public StorageAccountService(CloudTable cloudTable)
        {
            this.cloudTable = cloudTable;
        }

        public async Task<TableResult> AddEntityAsync(TableEntity entity)
        {
            await this.cloudTable.CreateIfNotExistsAsync();

            var insertOperation = TableOperation.Insert(entity);

            return await this.cloudTable.ExecuteAsync(insertOperation);
        }

        public async Task<TableResult> UpdateEntityAsync(TableEntity entity)
        {
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);

            return await this.cloudTable.ExecuteAsync(insertOrReplaceOperation);
        }

        public async Task<TEntity> GetEntityByIdAsync(Guid id)
        {
            var query = new TableQuery<TEntity>()
                .Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));

            return (await this.cloudTable.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();
        }

        public async Task<TableResult> DeleteEntityByIdAsync(Guid id)
        {
            var query = new TableQuery<TEntity>()
                .Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));

            var entity = (await this.cloudTable.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();

            var deleteOperation = TableOperation.Delete(entity);

            return await this.cloudTable.ExecuteAsync(deleteOperation);
        }

        public async Task<TableQuerySegment<TEntity>> ExecuteQuerySegmentedAsync(TableQuery<TEntity> query)
        {
            return await this.cloudTable.ExecuteQuerySegmentedAsync(query, null);
        }
    }
}
