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

        //[HttpGet]
        //public async Task<ActionResult> GetProjectsAsync([FromQuery] GetProjectModel filter)
        //{
        //    return Ok(await this.storageAccountService.GetProjectsAsync(filter));
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult> Get(Guid id)
        //{
        //    return Ok(await this.storageAccountService.GetProjectByIdAsync(id));
        //}

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

        //[HttpDelete("{code}")]
        //public async Task<ActionResult> Delete(int code)
        //{
        //    return Ok(await this.storageAccountService.DeleteProjectAsync(code));
        //}
    }
}
