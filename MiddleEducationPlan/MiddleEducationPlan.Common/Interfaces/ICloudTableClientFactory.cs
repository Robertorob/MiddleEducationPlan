using Microsoft.WindowsAzure.Storage.Table;

namespace MiddleEducationPlan.Common.Interfaces
{
    public interface ICloudTableClientFactory
    {
        public CloudTableClient GetCloudTableClient();
    }
}
