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

        public StorageAccountService(IConfiguration configuration, AzureKeyVaultService keyVaultServie)
        {
            this.configuration = configuration;
            this.keyVaultServie = keyVaultServie;
            this.storageAccountConnectionString = this.keyVaultServie.GetConnectionString("StorageAccountConnectionString");
            

        }

        public async Task<TableResult> AddProject(Project project)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageAccountConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("Project");
                await table.CreateIfNotExistsAsync();

                project.PartitionKey = project.Code.ToString();
                project.RowKey = Guid.NewGuid().ToString();
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
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageAccountConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("Project");
                await table.CreateIfNotExistsAsync();

                TableOperation incrementId = TableOperation.InsertOrReplace(project);

                return await table.ExecuteAsync(incrementId);
            }
            catch (Exception exc)
            {

                throw;
            }
        }

        public async Task<List<Project>> GetProjects()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageAccountConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Project");
            await table.CreateIfNotExistsAsync();

            TableQuery<Project> query = new TableQuery<Project>();
                    //.Where(TableQuery.GenerateFilterCondition("EntityName", QueryComparisons.NotEqual, "Id"));

            var g = await table.ExecuteQuerySegmentedAsync(query, null);

            return g.ToList();
        }
    }
}
