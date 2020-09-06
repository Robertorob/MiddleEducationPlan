using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Models.Task;
using MiddleEducationPlan.UnitTests.Helpers;
using MiddleEducationPlan.UnitTests.TaskUnitTests.Mock;
using MiddleEducationPlan.Web.Controllers;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using SystemTask = System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.TaskUnitTests
{
    public class TaskController_AddAsync_UnitTests
    {
        private TaskController taskController;
        private MockTaskCloudTableClient mockTaskCloudTableClient;
        private AddTaskModel addTaskModel;
        private AddTaskModel addEmptyNameTaskModel;

        [SetUp]
        public void Setup()
        {
            this.addTaskModel = new AddTaskModel
            {
                ProjectId = Guid.NewGuid(),
                Name = "add task"
            };
            this.addEmptyNameTaskModel = new AddTaskModel();

            (this.mockTaskCloudTableClient, this.taskController) = UnitTestSetupHelper.GetTaskControllerAndCloudTableClientMock();
        }

        [Test]
        public async SystemTask.Task AddAsync_ValidTask_Ok200()
        {
            var result = await this.taskController.AddAsync(this.addTaskModel) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.Created);
        }

        [Test]
        public async SystemTask.Task AddAsync_EmptyName_BadRequest()
        {
            var result = Validator.TryValidateObject(this.addEmptyNameTaskModel, new ValidationContext(this.addEmptyNameTaskModel, null, null), null, true);

            Assert.IsFalse(result);

            this.taskController.ModelState.AddModelError("Name", "The 'Name' field is required");
            var response = await this.taskController.AddAsync(this.addEmptyNameTaskModel) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.BadRequest);
        }
    }
}