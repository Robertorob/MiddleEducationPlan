using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleEducationPlan.Models;
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
        public async Task<ActionResult> Get()
        {
            return Ok(await _storageAccountService.GetProjects());
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult> Add([FromBody] AddProjectModel project)
        {
            return Ok(await _storageAccountService.AddProject(project));
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] UpdateProjectModel project)
        {
            return Ok(await _storageAccountService.UpdateProject(project));
        }

        [HttpDelete]
        [Route("delete/{projectCode}")]
        public async Task<ActionResult> Delete(int projectCode)
        {
            return Ok(await _storageAccountService.DeleteProject(projectCode));
        }
    }
}
