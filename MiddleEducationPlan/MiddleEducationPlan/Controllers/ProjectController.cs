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
        public ActionResult Get()
        {
            return Ok( new List<Project>() 
            {
                new Project{Name= "p", Code = 1},
                new Project{Name= "p2", Code = 2}
            });
        }

        [HttpPost]
        [Route("post")]
        public async Task<ActionResult> Post([FromBody] Project project)
        {
            //Project project = new Project
            //{
            //    Name = "Project",
            //    Code = 1
            //};
            var g = await _storageAccountService.AddProject(project);


            return Ok(g);
        }
    }
}
