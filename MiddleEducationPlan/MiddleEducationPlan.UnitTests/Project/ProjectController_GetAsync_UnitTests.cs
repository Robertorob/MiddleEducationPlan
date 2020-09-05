using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Services;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Project
{
    public class ProjectController_GetAsync_UnitTests
    {
        private ProjectController projectController;
        private Guid projectEntityId;

        [SetUp]
        public void Setup()
        {
            this.projectEntityId = Guid.NewGuid();
            var projectEntity = new BusinessLogic.TableEntities.ProjectEntity()
            {
                Id = this.projectEntityId,
                Name = "name"
            };

            var keyVaultMock = new Mock<ICloudTableClientFactory>();
            var taskServiceMock = new Mock<ITaskService>();

            var projectService = new ProjectService(taskServiceMock.Object, keyVaultMock.Object);

            //var projectServiceMock = new Mock<IProjectService>();
            //projectServiceMock.Setup(f => f.GetProjectByIdAsync(this.projectEntityId)).Returns(Task.FromResult(projectEntity));
            //projectServiceMock.Setup(f => f.GetProjectByIdAsync(this.projectEntityId)).Returns(Task.FromResult(projectEntity));

            this.projectController = new ProjectController(projectService);
        }

        [Test]
        public async Task GetAsync_ExistingProject_Ok200()
        {
            var result = await this.projectController.GetAsync(this.projectEntityId) as ObjectResult;

            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAsync_NonExistingProject_NotFound404()
        {
            var result = await this.projectController.GetAsync(Guid.NewGuid()) as NotFoundResult;

            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.NotFound);
        }
    }
}