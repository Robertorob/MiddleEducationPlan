using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.Models;
using MiddleEducationPlan.Models.Project;
using MiddleEducationPlan.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Services
{
    public abstract class StorageAccountService
    {
        private readonly IConfiguration configuration;
        private readonly AzureKeyVaultService keyVaultServie;
        private readonly string storageAccountConnectionString;
        private readonly CloudStorageAccount storageAccount;
        protected readonly CloudTableClient tableClient;

        public StorageAccountService(IConfiguration configuration, AzureKeyVaultService keyVaultServie)
        {
            this.configuration = configuration;
            this.keyVaultServie = keyVaultServie;
            this.storageAccountConnectionString = this.keyVaultServie.GetConnectionString("StorageAccountConnectionString");
            this.storageAccount = CloudStorageAccount.Parse(this.storageAccountConnectionString);
            this.tableClient = this.storageAccount.CreateCloudTableClient();
        }

        public async Task<TableResult> AddEntityAsync(TableEntity entity, CloudTable table)
        {
            await table.CreateIfNotExistsAsync();

            var insertOperation = TableOperation.Insert(entity);

            return await table.ExecuteAsync(insertOperation);
        }

        public async Task<TableResult> UpdateEntityAsync(TableEntity entity, CloudTable table)
        {
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);

            return await table.ExecuteAsync(insertOrReplaceOperation);
        }

        public async Task<TableEntity> GetEntityById(Guid id, CloudTable table)
        {
            var query = new TableQuery<ProjectEntity>()
                .Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));

            return (await table.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();
        }

        public async Task<TableResult> DeleteProjectAsync(int projectCode)
        {
            var table = this.tableClient.GetTableReference("Project");
            await table.CreateIfNotExistsAsync();

            var query = new TableQuery<ProjectEntity>()
                .Where(TableQuery.GenerateFilterConditionForInt("Code", QueryComparisons.Equal, projectCode));

            var projectEntity = (await table.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();

            var deleteOperation = TableOperation.Delete(projectEntity);

            return await table.ExecuteAsync(deleteOperation);
        }

        public async Task<List<ProjectEntity>> GetProjectsAsync(GetProjectModel filter)
        {
            var table = this.tableClient.GetTableReference("Project");
            await table.CreateIfNotExistsAsync();

            var query = new TableQuery<ProjectEntity>();

            var projects = await table.ExecuteQuerySegmentedAsync(query, null);

            return projects.ToList();
        }

        public async Task<List<ProjectEntity>> GetProjectByIdAsync(Guid id)
        {
            var table = this.tableClient.GetTableReference("Project");
            await table.CreateIfNotExistsAsync();

            var query = new TableQuery<ProjectEntity>().Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));

            var projects = await table.ExecuteQuerySegmentedAsync(query, null);

            return projects.ToList();
        }

        protected async Task<int> GetProjectCodeAndIncrementAsync(CloudTable table)
        {
            var query = new TableQuery<ProjectEntity>()
                    .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "0"));
            var idEntity = (await table.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();
            idEntity.Code++;
            var incrementCode = TableOperation.InsertOrReplace(idEntity);

            await table.ExecuteAsync(incrementCode);

            return idEntity.Code;
        }
    }
}
