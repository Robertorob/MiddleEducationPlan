using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiddleEducationPlan.BusinessLogic.Interfaces
{
    public interface IProjectService
    {
        public Task<TableResult> AddProjectAsync(AddProjectModel project);

        public Task<TableResult> UpdateProjectAsync(Guid id, UpdateProjectModel project);

        public Task<ProjectEntity> GetProjectByIdAsync(Guid id);

        public Task<List<ProjectEntity>> GetProjectsAsync(GetProjectModel filter);

        public Task<TableResult> DeleteProjectByIdAsync(Guid id);
    }
}
