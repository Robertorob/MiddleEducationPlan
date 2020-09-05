using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.Services;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.UnitTests.Project.Mock;
using MiddleEducationPlan.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Project
{
    public class ProjectController_GetProjectsAsync_UnitTests
    {
        private ProjectController projectController;
        private GetProjectModel getProjectWithCode3;
        private GetProjectModel getProject;
        private MockProjectCloudTableClient mockProjectCloudTableClient;

        [SetUp]
        public void Setup()
        {
            this.getProject = new GetProjectModel();
            this.getProjectWithCode3 = new GetProjectModel { Code = 3 };

            var taskServiceMock = new Mock<ITaskService>();

            this.mockProjectCloudTableClient = new MockProjectCloudTableClient(new Uri("https://educationplanstorageacc.table.core.windows.net/"),
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials());

            var cloudTableClientFactoryMock = new Mock<ICloudTableClientFactory>();
            cloudTableClientFactoryMock.Setup(f => f.GetCloudTableClient()).Returns(mockProjectCloudTableClient);

            var projectService = new ProjectService(taskServiceMock.Object, cloudTableClientFactoryMock.Object);

            this.projectController = new ProjectController(projectService);
        }

        [Test]
        public async Task GetProjectsAsync_AllProjects_Ok200()
        {
            var result = await this.projectController.GetProjectsAsync(this.getProject) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
            Assert.AreEqual(((List<ProjectEntity>)result.Value).Count, this.mockProjectCloudTableClient.mockProjectCloudTable.projects.Count);
        }

        [Test]
        public async Task GetProjectsAsync_NonExistingProject_NotFound404()
        {
            var result = await this.projectController.GetProjectsAsync(this.getProjectWithCode3) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.NotFound);
        }
    }
}