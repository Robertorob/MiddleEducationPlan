using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.Common.Interfaces;

namespace MiddleEducationPlan.Common.Services
{
    public class CloudTableClientFactory : ICloudTableClientFactory
    {
        const string STORAGE_ACCOUNT_CONNECTION_STRING_KEY = "StorageAccountConnectionString";
        private readonly IAzureKeyVaultService azureKeyVaultService;
        private readonly CloudTableClient cloudTableClient;

        public CloudTableClientFactory(IAzureKeyVaultService azureKeyVaultService)
        {
            this.azureKeyVaultService = azureKeyVaultService;
            var storageAccountConnectionString = this.azureKeyVaultService.GetConnectionString(STORAGE_ACCOUNT_CONNECTION_STRING_KEY);
            var storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            this.cloudTableClient = storageAccount.CreateCloudTableClient();
        }

        public CloudTableClient GetCloudTableClient()
        {
            return this.cloudTableClient;
        }
    }
}
