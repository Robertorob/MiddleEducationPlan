using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Models.Task;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.UnitTests.Helpers;
using MiddleEducationPlan.Web.Controllers;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.TaskUnitTests
{
    public class TaskController_UpdateAsync_UnitTests
    {
        private TaskController taskController;
        private UpdateTaskModel updateTaskModel;

        [SetUp]
        public void Setup()
        {
            this.updateTaskModel = new UpdateTaskModel
            {
                Name = "update"
            };

            this.taskController = UnitTestSetupHelper.GetTaskControllerMock();
        }

        [Test]
        public async Task UpdateAsync_ExistingTasks_Ok200()
        {
            var result = await this.taskController.UpdateAsync(UnitTestSetupHelper.ExistingTasks[0].Id, this.updateTaskModel) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
            Assert.AreEqual(((TaskEntity)result.Value).Name, this.updateTaskModel.Name);
        }

        [Test]
        public async Task UpdateAsync_NonExistingTask_NotFound404()
        {
            var result = await this.taskController.UpdateAsync(Guid.NewGuid(), this.updateTaskModel) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}