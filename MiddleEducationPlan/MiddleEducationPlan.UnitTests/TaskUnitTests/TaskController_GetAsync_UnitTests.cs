using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.UnitTests.Helpers;
using MiddleEducationPlan.UnitTests.TaskUnitTests.Mock;
using MiddleEducationPlan.Web.Controllers;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.TaskUnitTests
{
    public class TaskController_GetAsync_UnitTests
    {
        private TaskController taskController;
        private MockTaskCloudTableClient mockTaskCloudTableClient;

        [SetUp]
        public void Setup()
        {
            (this.mockTaskCloudTableClient, this.taskController) = UnitTestSetupHelper.GetTaskControllerAndCloudTableClientMock();
        }

        [Test]
        public async Task GetAsync_ExistingTask_Ok200()
        {
            var result = await this.taskController.GetAsync(this.mockTaskCloudTableClient.mockTaskCloudTable.tasks[0].Id) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAsync_NonExistingTask_NotFound404()
        {
            var result = await this.taskController.GetAsync(Guid.NewGuid()) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.NotFound);
        }
    }
}