using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Services;
using MiddleEducationPlan.Common.Services;
using MiddleEducationPlan.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests
{
    public class ProjectControllerUnitTests
    {
        private ProjectController projectController;
        private Guid projectEntityId;

        [SetUp]
        public void Setup()
        {
            this.projectEntityId = Guid.NewGuid();
            var projectEntity = new BusinessLogic.TableEntities.ProjectEntity()
            {
                Id = this.projectEntityId
            };

            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(f => f.GetProjectByIdAsync(this.projectEntityId)).Returns(Task.FromResult(projectEntity));

            this.projectController = new ProjectController(projectServiceMock.Object);
        }

        [Test]
        public async Task Get_ExistingProject_Ok200()
        {
            var result = await this.projectController.Get(this.projectEntityId) as ObjectResult;

            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
        }

        [Test]
        public async Task Get_NonExistingProject_NotFound404()
        {
            var result = await this.projectController.Get(Guid.NewGuid()) as ObjectResult;

            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Get_ExistingProjectByName_Ok200()
        {
            var result = await this.projectController.Get(this.projectEntityId) as ObjectResult;

            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
        }
    }
}