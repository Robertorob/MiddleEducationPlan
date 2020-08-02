using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.Models.Project;
using MiddleEducationPlan.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Services
{
    public class ProjectService : StorageAccountService
    {
        private const string ENTITY_NAME = "Project";
        private readonly CloudTable table;

        public ProjectService(IConfiguration configuration, AzureKeyVaultService keyVaultServie) : base(configuration, keyVaultServie) 
        { 
            this.table = this.tableClient.GetTableReference(ProjectService.ENTITY_NAME);
        }

        public async Task<TableResult> AddProjectAsync(AddProjectModel project)
        {
            Guid id = Guid.NewGuid();
            int code = await GetProjectCodeAndIncrementAsync(table);
            return await AddEntityAsync(new ProjectEntity 
            { 
                Id = id,
                Code = code,
                PartitionKey = code.ToString(),
                RowKey = id.ToString(),
                Name = project.Name
            }, this.table);
        }

        public async Task<TableResult> UpdateProjectAsync(Guid id, UpdateProjectModel project)
        {
            var projectEntity = await this.GetEntityById<ProjectEntity>(id, this.table);

            if (projectEntity == null)
                return null;

            projectEntity.Name = project.Name;

            return await this.UpdateEntityAsync(projectEntity, this.table);
        }

        public async Task<ProjectEntity> GetProjectByIdAsync(Guid id)
        {
            return await GetEntityById<ProjectEntity>(id, this.table);
        }
    }
}
