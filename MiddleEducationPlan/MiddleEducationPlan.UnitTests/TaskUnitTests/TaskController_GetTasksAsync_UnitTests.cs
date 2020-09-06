using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Models.Task;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.UnitTests.Helpers;
using MiddleEducationPlan.UnitTests.TaskUnitTests.Mock;
using MiddleEducationPlan.Web.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.TaskUnitTests
{
    public class TaskController_GetTasksAsync_UnitTests
    {
        private TaskController taskController;
        private GetTaskModel getTask;
        private MockTaskCloudTableClient mockTaskCloudTableClient;

        [SetUp]
        public void Setup()
        {
            this.getTask = new GetTaskModel();

            (this.mockTaskCloudTableClient, this.taskController) = UnitTestSetupHelper.GetTaskControllerAndCloudTableClientMock();
        }

        [Test]
        public async Task GetTasksAsync_AllTasks_Ok200()
        {
            var result = await this.taskController.GetTasksAsync(this.getTask) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
            Assert.AreEqual(((List<TaskEntity>)result.Value).Count, this.mockTaskCloudTableClient.mockTaskCloudTable.tasks.Count);
        }
    }
}