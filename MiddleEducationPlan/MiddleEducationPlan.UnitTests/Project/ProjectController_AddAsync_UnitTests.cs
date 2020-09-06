using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.Services;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.UnitTests.Project.Mock;
using MiddleEducationPlan.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Project
{   
    public class ProjectController_AddAsync_UnitTests
    {
        private ProjectController projectController;
        private MockProjectCloudTableClient mockProjectCloudTableClient;
        private AddProjectModel addProjectModel;
        private AddProjectModel addEmptyNameProject;

        [SetUp]
        public void Setup()
        {
            this.addProjectModel = new AddProjectModel
            {
                Name = "add project"
            };
            this.addEmptyNameProject = new AddProjectModel();

            var taskServiceMock = new Mock<ITaskService>();

            this.mockProjectCloudTableClient = new MockProjectCloudTableClient(new Uri("https://educationplanstorageacc.table.core.windows.net/"),
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials());

            var cloudTableClientFactoryMock = new Mock<ICloudTableClientFactory>();
            cloudTableClientFactoryMock.Setup(f => f.GetCloudTableClient()).Returns(mockProjectCloudTableClient);

            var projectService = new ProjectService(taskServiceMock.Object, cloudTableClientFactoryMock.Object);

            this.projectController = new ProjectController(projectService);    
        }

        [Test]
        public async Task AddAsync_ValidProject_Ok200()
        {
            var result = await this.projectController.AddAsync(this.addProjectModel) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.Created);
        }

        [Test]
        public async Task AddAsync_EmptyName_BadRequest()
        {
            var result = Validator.TryValidateObject(this.addEmptyNameProject, new ValidationContext(this.addEmptyNameProject, null, null), null, true);

            Assert.IsFalse(result);

            this.projectController.ModelState.AddModelError("Name", "The 'Name' field is required");
            var response = await this.projectController.AddAsync(this.addEmptyNameProject) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.BadRequest);
        }
    }
}