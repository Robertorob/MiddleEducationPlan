using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Extensions;
using System;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
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

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] AddProjectModel project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = (await this.projectService.AddProjectAsync(project)).Result;

            return Created($"{Constants.ProjectBaseAddress}/{((ProjectEntity) result).Id}", result);
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
