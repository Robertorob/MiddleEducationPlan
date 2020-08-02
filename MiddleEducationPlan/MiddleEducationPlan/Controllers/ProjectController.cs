using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleEducationPlan.Extensions;
using MiddleEducationPlan.Models;
using MiddleEducationPlan.Models.Project;
using MiddleEducationPlan.Services;

namespace MiddleEducationPlan.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> logger;
        private readonly ProjectService projectService;

        public ProjectController(ILogger<ProjectController> logger, ProjectService projectService)
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

            return Ok(await this.projectService.AddProjectAsync(project));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateProjectModel project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await this.projectService.UpdateProjectAsync(id, project);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await this.projectService.GetProjectByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(await this.projectService.DeleteProjectByIdAsync(id));
        }
    }
}
