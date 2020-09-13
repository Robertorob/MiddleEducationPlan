using System;
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

            return View(result.OrderByDescending(f => f.Code));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ResultModel<ProjectEntity>> CreatePost(AddProjectModel project)
        {
            if (!ModelState.IsValid)
                return new ResultModel<ProjectEntity>
                {
                    Status = Status.Error,
                    ErrorMessage = "Model is invalid"
                };

            if (ModelState.IsValid)
            {
                var projectEntity = (await this.projectService.AddProjectAsync(project)).Result;
                if (projectEntity == null)
                {
                    return new ResultModel<ProjectEntity>
                    {
                        Status = Status.Error,
                        ErrorMessage = "Unexpected error"
                    };
                }
                return new ResultModel<ProjectEntity>
                {
                    Status = Status.Success,
                    Value = (ProjectEntity)projectEntity
                };
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GetProjectsAsync([FromQuery] GetProjectModel filter)
        {
            var result = await this.projectService.GetProjectsAsync(filter);

            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(Guid id)
        {
            var result = await this.projectService.GetProjectByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] UpdateProjectModel project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await this.projectService.UpdateProjectAsync(id, project);

            if (result == null)
                return NotFound();

            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var result = await this.projectService.GetProjectByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok((await this.projectService.DeleteProjectByIdAsync(id)).Result);
        }
    }
}
