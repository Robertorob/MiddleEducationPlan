
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace MiddleEducationPlan.UnitTests.TaskUnitTests.Mock
{
    public class MockTaskCloudTableClient : CloudTableClient
    {
        public MockTaskCloudTable mockTaskCloudTable;

        public MockTaskCloudTableClient(Uri baseUri, StorageCredentials credentials) : base(baseUri, credentials)
        {
            this.mockTaskCloudTable = new MockTaskCloudTable(new Uri("https://educationplanstorageacc.table.core.windows.net/Task"));
        }

        public override CloudTable GetTableReference(string tableName)
        {
            return this.mockTaskCloudTable;
        }
    }
}
