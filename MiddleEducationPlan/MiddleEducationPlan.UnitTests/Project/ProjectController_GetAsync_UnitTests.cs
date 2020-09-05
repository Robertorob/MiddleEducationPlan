using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Services;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.UnitTests.Project.Mock;
using MiddleEducationPlan.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Project
{
    /// <summary>
    /// Mock class for CloudTable object
    /// </summary>
    

    public class ProjectController_GetAsync_UnitTests
    {
        private ProjectController projectController;
        private MockProjectCloudTableClient mockProjectCloudTableClient;

        [SetUp]
        public void Setup()
        {
            var taskServiceMock = new Mock<ITaskService>();

            this.mockProjectCloudTableClient = new MockProjectCloudTableClient(new Uri("https://educationplanstorageacc.table.core.windows.net/"),
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials());

            var cloudTableClientFactoryMock = new Mock<ICloudTableClientFactory>();
            cloudTableClientFactoryMock.Setup(f => f.GetCloudTableClient()).Returns(mockProjectCloudTableClient);

            var projectService = new ProjectService(taskServiceMock.Object, cloudTableClientFactoryMock.Object);

            this.projectController = new ProjectController(projectService);
        }

        [Test]
        public async Task GetAsync_ExistingProject_Ok200()
        {
            var result = await this.projectController.GetAsync(this.mockProjectCloudTableClient.mockProjectCloudTable.projects[0].Id) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAsync_NonExistingProject_NotFound404()
        {
            var result = await this.projectController.GetAsync(Guid.NewGuid()) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.NotFound);
        }
    }
}