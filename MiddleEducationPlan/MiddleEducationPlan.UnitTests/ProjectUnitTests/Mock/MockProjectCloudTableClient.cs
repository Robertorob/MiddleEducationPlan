
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
