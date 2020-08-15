using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using MiddleEducationPlan.Common.Interfaces;

namespace MiddleEducationPlan.Common.Services
{
    public class AzureKeyVaultService : IAzureKeyVaultService
    {
        private readonly IConfiguration configuration;
        private readonly KeyVaultClient keyVaultClient;
        public AzureKeyVaultService(IConfiguration configuration)
        {
            this.configuration = configuration;
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            this.keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        }

        public string GetConnectionString(string connectionStringName)
        {
            var keyVaultSection = this.configuration.GetSection("KeyVault");
            string keyVaultName = keyVaultSection["Name"];
            string secretKey = keyVaultSection.GetSection("SecretKeys")[connectionStringName];
#if RELEASE
            
            string connectionString = this.keyVaultClient.GetSecretAsync($"https://{keyVaultName}.vault.azure.net/secrets/{secretKey}").Result.Value;
#else
            string connectionString = this.configuration.GetConnectionString(connectionStringName);
#endif
            return connectionString;
        }
    }
}
