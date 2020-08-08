using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Common.Services
{
    public abstract class StorageAccountService<TEntity> where TEntity : TableEntity, new()
    {
        private readonly AzureKeyVaultService keyVaultServie;
        private readonly string storageAccountConnectionString;
        private readonly CloudStorageAccount storageAccount;
        protected readonly CloudTableClient tableClient;
        private const string STORAGE_ACCOUNT_CONNECTION_STRING_KEY = "StorageAccountConnectionString";

        public StorageAccountService(AzureKeyVaultService keyVaultServie)
        {
            this.keyVaultServie = keyVaultServie;
            this.storageAccountConnectionString = this.keyVaultServie.GetConnectionString(STORAGE_ACCOUNT_CONNECTION_STRING_KEY);
            this.storageAccount = CloudStorageAccount.Parse(this.storageAccountConnectionString);
            this.tableClient = this.storageAccount.CreateCloudTableClient();
        }

        protected async Task<TableResult> AddEntityAsync(TableEntity entity, CloudTable table)
        {
            await table.CreateIfNotExistsAsync();

            var insertOperation = TableOperation.Insert(entity);

            return await table.ExecuteAsync(insertOperation);
        }

        protected async Task<TableResult> UpdateEntityAsync(TableEntity entity, CloudTable table)
        {
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);

            return await table.ExecuteAsync(insertOrReplaceOperation);
        }

        protected async Task<TEntity> GetEntityById(Guid id, CloudTable table)
        {
            var query = new TableQuery<TEntity>()
                .Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));

            return (await table.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();
        }

        protected async Task<TableResult> DeleteEntityByIdAsync(Guid id, CloudTable table)
        {
            var query = new TableQuery<TEntity>()
                .Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));

            var entity = (await table.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();

            var deleteOperation = TableOperation.Delete(entity);

            return await table.ExecuteAsync(deleteOperation);
        }
    }
}
