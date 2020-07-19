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

                TableQuery<Project> query = new TableQuery<Project>()
                    .Where(TableQuery.GenerateFilterCondition("EntityName", QueryComparisons.Equal, "Id"));

                var idEntity = (await table.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();

                //TableOperation readId = TableOperation.Retrieve("0", "0");

                //var idEntity = (DynamicTableEntity) table.ExecuteAsync(readId).Result.Result;
                //var prop = idEntity.Properties.Where(f => f.Key == "Id").FirstOrDefault();
                //idEntity.Properties.Remove(prop.Key);
                //prop.Value.Int32Value++;
                //idEntity.Properties.Add(prop.Key, prop.Value);

                idEntity.Id++;

                TableOperation incrementId = TableOperation.InsertOrReplace(idEntity);

                await table.ExecuteAsync(incrementId);

                project.RowKey = idEntity.Id.ToString();// prop.Value.Int32Value.ToString();
                TableOperation insert = TableOperation.Insert(project);
                var g = await table.ExecuteAsync(insert);

                return g;
            }
            catch (Exception exc)
            {

                throw;
            }
        }

        public async Task<TableResult> GetProjects()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageAccountConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Project");
            await table.CreateIfNotExistsAsync();
            TableOperation insert = TableOperation.Retrieve("Name", "Name");
            return await table.ExecuteAsync(insert);
        }
    }
}
