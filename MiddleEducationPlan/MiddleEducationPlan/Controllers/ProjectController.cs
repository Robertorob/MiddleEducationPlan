using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Extensions;

namespace MiddleEducationPlan.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> logger;
        private readonly IProjectService projectService;

        public ProjectController(ILogger<ProjectController> logger, IProjectService projectService)
        {
            this.logger = logger;
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
        public async Task<ActionResult> Get(Guid id)
        {
            var result = await this.projectService.GetProjectByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] AddProjectModel project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = (await this.projectService.AddProjectAsync(project)).Result;

            return Created($"{Constants.ProjectBaseAddress}/{((ProjectEntity) result).Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateProjectModel project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await this.projectService.UpdateProjectAsync(id, project);

            if (result == null)
                return NotFound();

            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await this.projectService.GetProjectByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok((await this.projectService.DeleteProjectByIdAsync(id)).Result);
        }
    }
}
