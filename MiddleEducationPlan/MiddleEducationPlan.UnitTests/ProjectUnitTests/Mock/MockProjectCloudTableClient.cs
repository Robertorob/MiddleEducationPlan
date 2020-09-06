
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace MiddleEducationPlan.UnitTests.Project.Mock
{
    public class MockProjectCloudTableClient : CloudTableClient
    {
        public MockProjectCloudTable mockProjectCloudTable;

        public MockProjectCloudTableClient(Uri baseUri, StorageCredentials credentials) : base(baseUri, credentials)
        {
            this.mockProjectCloudTable = new MockProjectCloudTable(new Uri("https://educationplanstorageacc.table.core.windows.net/Project"));
        }

        public override CloudTable GetTableReference(string tableName)
        {
            return this.mockProjectCloudTable;
        }
    }
}
