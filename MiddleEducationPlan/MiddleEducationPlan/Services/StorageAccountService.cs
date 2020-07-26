using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.Models;
using MiddleEducationPlan.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Services
{
    public class StorageAccountService
    {
        private readonly IConfiguration configuration;
        private readonly AzureKeyVaultService keyVaultServie;
        private readonly string storageAccountConnectionString;
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudTableClient tableClient;

        public StorageAccountService(IConfiguration configuration, AzureKeyVaultService keyVaultServie)
        {
            this.configuration = configuration;
            this.keyVaultServie = keyVaultServie;
            this.storageAccountConnectionString = this.keyVaultServie.GetConnectionString("StorageAccountConnectionString");
            this.storageAccount = CloudStorageAccount.Parse(this.storageAccountConnectionString);
            this.tableClient = this.storageAccount.CreateCloudTableClient();
        }

        public async Task<TableResult> AddProject(AddProjectModel project)
        {
            var table = this.tableClient.GetTableReference("Project");
            await table.CreateIfNotExistsAsync();

            var projectEntity = new ProjectEntity();

            projectEntity.Id = Guid.NewGuid();
            projectEntity.Code = await GetProjectCodeAndIncrement(table);
            projectEntity.PartitionKey = projectEntity.Code.ToString();
            projectEntity.RowKey = projectEntity.Id.ToString();
            projectEntity.Name = project.Name;

            var insert = TableOperation.Insert(projectEntity);

            return await table.ExecuteAsync(insert);
        }

        public async Task<TableResult> UpdateProject(UpdateProjectModel project)
        {
            var table = this.tableClient.GetTableReference("Project");
            await table.CreateIfNotExistsAsync();

            var query = new TableQuery<ProjectEntity>()
                .Where(TableQuery.GenerateFilterConditionForInt("Code", QueryComparisons.Equal, project.Code));

            var asdf = (await table.ExecuteQuerySegmentedAsync(query, null));

            var projectEntity = asdf.Results.FirstOrDefault();

            projectEntity.Name = project.Name;

            TableOperation tableOperation = TableOperation.InsertOrReplace(projectEntity);

            return await table.ExecuteAsync(tableOperation);
        }

        public async Task<List<ProjectEntity>> GetProjects()
        {
            var table = this.tableClient.GetTableReference("Project");
            await table.CreateIfNotExistsAsync();

            var query = new TableQuery<ProjectEntity>();

            var projects = await table.ExecuteQuerySegmentedAsync(query, null);

            return projects.ToList();
        }

        private async Task<int> GetProjectCodeAndIncrement(CloudTable table)
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
