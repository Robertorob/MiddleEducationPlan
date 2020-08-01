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
        private readonly ILogger<ProjectController> _logger;
        private readonly StorageAccountService _storageAccountService;

        public ProjectController(ILogger<ProjectController> logger, StorageAccountService storageAccountService)
        {
            _logger = logger;
            _storageAccountService = storageAccountService;
        }

        [HttpGet]
        public async Task<ActionResult> GetProjectsAsync([FromQuery] GetProjectModel filter)
        {
            return Ok(await _storageAccountService.GetProjectsAsync(filter));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _storageAccountService.GetProjectByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] AddProjectModel project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            return Ok(await _storageAccountService.AddProjectAsync(project));
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateProjectModel project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            return Ok(await _storageAccountService.UpdateProjectAsync(project));
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult> Delete(int code)
        {
            return Ok(await _storageAccountService.DeleteProjectAsync(code));
        }
    }
}
