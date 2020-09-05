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
        public List<ProjectEntity> projects = new List<ProjectEntity> {
            new ProjectEntity
            {
                Id = new Guid("f075cc3a-cd87-41ac-be51-a071ebf87641"),
                Code = 0
            },
            new ProjectEntity
            {
                Id = new Guid("08a7db72-02a7-4a7d-9edd-11ef91133ada"),
                Code = 1
            },
            new ProjectEntity
            {
                Id = new Guid("1c7fb7c6-7c13-4926-91af-435182b10667"),
                Code = 2
            }
        };

        public MockProjectCloudTable(Uri tableAddress) : base(tableAddress) { }

        public async override Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            if(operation.OperationType == TableOperationType.InsertOrReplace)
            {
                return await Task.FromResult(new TableResult
                {
                    Result = operation.Entity,
                    HttpStatusCode = 200
                });
            }

            if (operation.OperationType == TableOperationType.Insert)
            {
                return await Task.FromResult(new TableResult
                {
                    Result = operation.Entity,
                    HttpStatusCode = 201
                });
            }

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

            var queryString = query.FilterString;

            if (!string.IsNullOrEmpty(queryString))
            {
                if (queryString.Contains("Id eq guid'"))
                {
                    var guid = new Guid(queryString.Replace("Id eq guid'", "").Replace("'", ""));
                    return ctor.Invoke(new object[]
                    {
                        this.projects.Where(f => f.Id == guid).ToList()
                    }) as TableQuerySegment<TEntity>;
                }
                if(queryString.Contains("Code eq "))
                {
                    queryString = queryString.Replace("Code eq ", "");
                    int code = int.Parse(queryString);
                    return ctor.Invoke(new object[]
                    {
                        this.projects.Where(f => f.Code == code).ToList()
                    }) as TableQuerySegment<TEntity>;
                }
                if(queryString.Contains("RowKey eq '0'"))
                {
                    int maxCode = this.projects.Max(f => f.Code);
                    return ctor.Invoke(new object[]
                    {
                        this.projects.Where(f => f.Code == maxCode).ToList()
                    }) as TableQuerySegment<TEntity>;
                }
            }
            else
            {
                return ctor.Invoke(new object[] { this.projects }) as TableQuerySegment<TEntity>;
            }

            return await Task.FromResult(ctor.Invoke(new object[] { new List<ProjectEntity>() { } }) as TableQuerySegment<TEntity>);
        }

        public async override Task<bool> CreateIfNotExistsAsync()
        {
            return await Task.FromResult(true);
        }
    }
}
