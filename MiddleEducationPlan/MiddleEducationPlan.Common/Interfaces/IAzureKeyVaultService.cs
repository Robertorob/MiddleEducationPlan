namespace MiddleEducationPlan.Common.Interfaces
{
    public interface IAzureKeyVaultService
    {
        public string GetConnectionString(string connectionStringName);
    }
}
