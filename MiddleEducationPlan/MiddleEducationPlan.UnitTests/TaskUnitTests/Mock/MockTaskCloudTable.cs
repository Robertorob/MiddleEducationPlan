using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.TaskUnitTests.Mock
{
    public class MockTaskCloudTable : CloudTable
    {
        public List<TaskEntity> tasks = new List<TaskEntity> {
            new TaskEntity
            {
                Id = new Guid("f075cc3a-cd87-41ac-be51-a071ebf87641"),
                Name = "name 0",
                ETag= "*"
            },
            new TaskEntity
            {
                Id = new Guid("08a7db72-02a7-4a7d-9edd-11ef91133ada"),
                Name = "name 1",
                ETag= "*"
            },
            new TaskEntity
            {
                Id = new Guid("1c7fb7c6-7c13-4926-91af-435182b10667"),
                Name = "name 2",
                ETag= "*"
            }
        };

        public MockTaskCloudTable(Uri tableAddress) : base(tableAddress) { }

        public async override Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            if(operation.OperationType == TableOperationType.InsertOrReplace)
            {
                return await System.Threading.Tasks.Task.FromResult(new TableResult
                {
                    Result = operation.Entity,
                    HttpStatusCode = 200
                });
            }

            if (operation.OperationType == TableOperationType.Insert)
            {
                return await System.Threading.Tasks.Task.FromResult(new TableResult
                {
                    Result = operation.Entity,
                    HttpStatusCode = 201
                });
            }

            return await System.Threading.Tasks.Task.FromResult(new TableResult
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

            var queryString = query.FilterString;

            if (!string.IsNullOrEmpty(queryString))
            {
                if (queryString.Contains("Id eq guid'"))
                {
                    var guid = new Guid(queryString.Replace("Id eq guid'", "").Replace("'", ""));
                    return ctor.Invoke(new object[]
                    {
                        this.tasks.Where(f => f.Id == guid).ToList()
                    }) as TableQuerySegment<TEntity>;
                }
            }
            else
            {
                return ctor.Invoke(new object[] { this.tasks }) as TableQuerySegment<TEntity>;
            }

            return await System.Threading.Tasks.Task.FromResult(ctor.Invoke(new object[] { new List<TaskEntity>() { } }) as TableQuerySegment<TEntity>);
        }

        public async override Task<bool> CreateIfNotExistsAsync()
        {
            return await System.Threading.Tasks.Task.FromResult(true);
        }
    }
}
