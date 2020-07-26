using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.Models;
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

        public async Task<TableResult> AddProject(Project project)
        {
            try
            {
                CloudTable table = this.tableClient.GetTableReference("Project");
                await table.CreateIfNotExistsAsync();

                project.Id = Guid.NewGuid();
                project.PartitionKey = project.Code.ToString();
                project.RowKey = project.Id.ToString();
                TableOperation insert = TableOperation.Insert(project);

                return await table.ExecuteAsync(insert);
            }
            catch (Exception exc)
            {

                throw;
            }
        }

        public async Task<TableResult> UpdateProject(Project project)
        {
            try
            {
                CloudTable table = this.tableClient.GetTableReference("Project");
                await table.CreateIfNotExistsAsync();

                TableOperation tableOperation = TableOperation.InsertOrReplace(project);

                return await table.ExecuteAsync(tableOperation);
            }
            catch (Exception exc)
            {

                throw;
            }
        }

        public async Task<List<Project>> GetProjects()
        {
            CloudTable table = this.tableClient.GetTableReference("Project");
            await table.CreateIfNotExistsAsync();

            TableQuery<Project> query = new TableQuery<Project>();
                    //.Where(TableQuery.GenerateFilterCondition("EntityName", QueryComparisons.NotEqual, "Id"));

            var g = await table.ExecuteQuerySegmentedAsync(query, null);

            return g.ToList();
        }
    }
}
