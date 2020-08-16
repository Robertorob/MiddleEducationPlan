using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Project
{
    public class ProjectController_GetProjectsAsync_UnitTests
    {
        private ProjectController projectController;
        private List<ProjectEntity> projects;
        private GetProjectModel getProjectWithCode3;
        private GetProjectModel getProject;

        [SetUp]
        public void Setup()
        {
            this.projects = new List<ProjectEntity>();

            for (int i = 0; i < 3; i++)
            {
                var projectEntityId = Guid.NewGuid();
                var projectEntity = new ProjectEntity()
                {
                    Id = projectEntityId,
                    Code = i,
                    Name = $"name {i}"
                };
                this.projects.Add(projectEntity);
            }

            var projectServiceMock = new Mock<IProjectService>();

            this.getProject = new GetProjectModel();
            projectServiceMock.Setup(f => f.GetProjectsAsync(this.getProject)).Returns(Task.FromResult(this.projects));

            this.getProjectWithCode3 = new GetProjectModel { Code = 3 };
            projectServiceMock.Setup(f => f.GetProjectsAsync(this.getProjectWithCode3)).Returns(Task.FromResult(this.projects.Where(f => f.Code == 3).ToList()));

            this.projectController = new ProjectController(projectServiceMock.Object);
        }

        [Test]
        public async Task GetProjectsAsync_AllProjects_Ok200()
        {
            var result = await this.projectController.GetProjectsAsync(this.getProject) as ObjectResult;

            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
        }

        [Test]
        public async Task GetProjectsAsync_NonExistingProject_NotFound404()
        {
            var result = await this.projectController.GetProjectsAsync(this.getProjectWithCode3) as NotFoundResult;

            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.NotFound);
        }
    }
}