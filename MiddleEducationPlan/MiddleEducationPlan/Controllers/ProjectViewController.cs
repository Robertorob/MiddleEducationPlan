using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Extensions;
using MiddleEducationPlan.Web.Models;

namespace MiddleEducationPlan.Web.Controllers
{
    //[Route("ProjectView")]
    public class ProjectViewController : Controller
    {
        private readonly IProjectService projectService;

        public ProjectViewController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await this.projectService.GetProjectsAsync(new GetProjectModel());

            if (result.Count == 0)
                return NotFound();

            return View(result.OrderByDescending(f => f.Timestamp).Select(f => new ProjectModel
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                ProjectType = ((ProjectType)f.ProjectTypeInteger).ToString(),
                CreatedAt = f.Timestamp.UtcDateTime
            }));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ResultModel<UpdateProjectModel>> CreatePost(AddProjectModel project)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new ResultModel<UpdateProjectModel>
                    {
                        Status = Status.Error,
                        ErrorMessage = "Model is invalid"
                    };

                if (ModelState.IsValid)
                {
                    var projectEntity = (ProjectEntity)(await this.projectService.AddProjectAsync(project)).Result;
                    if (projectEntity == null)
                    {
                        return new ResultModel<UpdateProjectModel>
                        {
                            Status = Status.Error,
                            ErrorMessage = "Internal server error"
                        };
                    }
                    return new ResultModel<UpdateProjectModel>
                    {
                        Status = Status.Success,
                        Value = new UpdateProjectModel
                        {
                            Id = projectEntity.Id
                        }
                    };
                }
            }
            catch (Exception exc)
            {
                return new ResultModel<UpdateProjectModel>
                {
                    Status = Status.Error,
                    ErrorMessage = $"Internal server error: {exc.Message}"
                };
            }

            return null;
        }

        public async Task<IActionResult> UpdateAsync(Guid id)
        {
            var projectEntity = await this.projectService.GetProjectByIdAsync(id);

            var model = new UpdateProjectModel
            {
                Id = projectEntity.Id,
                Name = projectEntity.Name,
                Description = projectEntity.Description,
                ProjectType = (ProjectType)projectEntity.ProjectTypeInteger
            };

            return View(model);
        }

        [HttpGet]
        public ResultModel<SelectModel<ProjectTypeModel>> GetProjectTypes()
        {
            try
            {
                var values = Enum.GetValues(typeof(ProjectType));
                var selectModel = new SelectModel<ProjectTypeModel>();
                selectModel.Values = new List<ProjectTypeModel>();

                foreach (var item in values)
                {
                    selectModel.Values.Add(new ProjectTypeModel
                    {
                        Name = Enum.GetName(typeof(ProjectType), item),
                        Value = (int)item
                    });
                }

                return new ResultModel<SelectModel<ProjectTypeModel>>
                {
                    Value = selectModel,
                    Status = Status.Success
                };
            }
            catch (Exception exc)
            {
                return new ResultModel<SelectModel<ProjectTypeModel>>
                {
                    Status = Status.Error,
                    ErrorMessage = exc.Message
                };
            }
        }

        [HttpGet("ProjectView/GetAsync/{id}")]
        public async Task<ResultModel<UpdateProjectModel>> GetAsync(Guid id)
        {
            var result = await this.projectService.GetProjectByIdAsync(id);

            if (result == null)
            {
                return new ResultModel<UpdateProjectModel>
                {
                    Status = Status.Error,
                    ErrorMessage = $"Could not find the project with id {id}"
                };
            }

            return new ResultModel<UpdateProjectModel>
            {
                Status = Status.Success,
                Value = new UpdateProjectModel
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    ProjectType = (ProjectType)result.ProjectTypeInteger
                }
            };
        }


        [HttpPost]
        public async Task<ResultModel<UpdateProjectModel>> UpdatePost(UpdateProjectModel project)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new ResultModel<UpdateProjectModel>
                    {
                        Status = Status.Error,
                        ErrorMessage = "Model is invalid"
                    };

                if (ModelState.IsValid)
                {
                    var projectEntity = (ProjectEntity)(await this.projectService.UpdateProjectAsync(project.Id ?? Guid.NewGuid(), project)).Result;
                    if (projectEntity == null)
                    {
                        return new ResultModel<UpdateProjectModel>
                        {
                            Status = Status.Error,
                            ErrorMessage = "Project not found"
                        };
                    }
                    return new ResultModel<UpdateProjectModel>
                    {
                        Status = Status.Success
                    };
                }
            }
            catch (Exception exc)
            {
                return new ResultModel<UpdateProjectModel>
                {
                    Status = Status.Error,
                    ErrorMessage = $"Internal server error: {exc.Message}"
                };
            }

            return null;
        }

        //[HttpGet("ProjectView/Delete/{id}")]
        public async Task<ResultModel<Guid>> DeleteAsync(Guid id)
        {
            var result = await this.projectService.GetProjectByIdAsync(id);

            if (result == null)
                return new ResultModel<Guid>
                {
                    Status = Status.Error,
                    ErrorMessage = "Not found"
                };

            try
            {
                await this.projectService.DeleteProjectByIdAsync(id);

                return new ResultModel<Guid>
                {
                    Status = Status.Success,
                    Value = id
                };
            }
            catch (Exception exc)
            {
                return new ResultModel<Guid>
                {
                    Status = Status.Error,
                    ErrorMessage = exc.Message
                };
            }
        }
    }
}
