using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;

namespace MiddleEducationPlan.Common.Interfaces
{
    public interface IAzureKeyVaultService
    {
        public string GetConnectionString(string connectionStringName);
    }
}
