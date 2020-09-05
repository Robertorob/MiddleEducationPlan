
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

        public List<Guid> Guids = new List<Guid> { 
            new Guid("f075cc3a-cd87-41ac-be51-a071ebf87641"),
            new Guid("08a7db72-02a7-4a7d-9edd-11ef91133ada"),
            new Guid("1c7fb7c6-7c13-4926-91af-435182b10667")
        };

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
