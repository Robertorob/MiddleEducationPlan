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
    public class MockProjectCloudTable : CloudTable
    {
        public List<Guid> Guids = new List<Guid> { 
            new Guid("f075cc3a-cd87-41ac-be51-a071ebf87641"),
            new Guid("08a7db72-02a7-4a7d-9edd-11ef91133ada"),
            new Guid("1c7fb7c6-7c13-4926-91af-435182b10667")
        };

        public MockProjectCloudTable(Uri tableAddress) : base(tableAddress)
        { }

        public async override Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            return await Task.FromResult(new TableResult
            {
                Result = new object(),
                HttpStatusCode = 200
            });
        }

        public async override Task<TableQuerySegment<TEntity>> ExecuteQuerySegmentedAsync<TEntity>(TableQuery<TEntity> query, TableContinuationToken token)
        {
            var ctor = typeof(TableQuerySegment<TEntity>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            string queryString = query.FilterString;

            if(queryString.Contains("Id eq guid'"))
            {
                foreach (var guid in this.Guids)
                {
                    if (queryString.Contains(guid.ToString()))
                    {
                        var mockQuerySegment = ctor.Invoke(new object[] { new List<ProjectEntity>()
                        {
                            new ProjectEntity
                            {
                                Id = guid
                            }
                        }
                        }) as TableQuerySegment<TEntity>;

                        return await Task.FromResult(mockQuerySegment);
                    }
                }
            }

            return await Task.FromResult(ctor.Invoke(new object[] { new List<ProjectEntity>() { } }) as TableQuerySegment<TEntity>);
        }
    }
}
