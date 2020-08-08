using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleEducationPlan.BusinessLogic.Models.Task;
using MiddleEducationPlan.Extensions;
using MiddleEducationPlan.Services;

namespace MiddleEducationPlan.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> logger;
        private readonly TaskService taskService;
        private readonly ProjectService projectService;

        public TaskController(ILogger<TaskController> logger, TaskService taskService, ProjectService projectService)
        {
            this.logger = logger;
            this.taskService = taskService;
            this.projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult> GetTasksAsync([FromQuery] GetTaskModel filter)
        {
            var result = await this.taskService.GetTasksAsync(filter);

            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var result = await this.taskService.GetTaskByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] AddTaskModel task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var project = await this.projectService.GetProjectByIdAsync(task.ProjectId ?? new Guid());
            if (project == null)
                return BadRequest($"Project with ID \"{task.ProjectId}\" does not exist.");

            return Ok((await this.taskService.AddTaskAsync(task)).Result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateTaskModel task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await this.taskService.UpdateTaskAsync(id, task);

            if (result == null)
                return NotFound();

            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await this.taskService.GetTaskByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok((await this.taskService.DeleteTaskByIdAsync(id)).Result);
        }
    }
}
