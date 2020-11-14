using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.UnitTests.Helpers;
using MiddleEducationPlan.UnitTests.Project.Mock;
using MiddleEducationPlan.Web.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Project
{
    public class ProjectController_GetProjectsAsync_UnitTests
    {
        private ProjectController projectController;
        private GetProjectModel getProjectWithSomeName;
        private GetProjectModel getProject;
        private MockProjectCloudTableClient mockProjectCloudTableClient;

        [SetUp]
        public void Setup()
        {
            this.getProject = new GetProjectModel();
            this.getProjectWithSomeName = new GetProjectModel { Name = "nonExistingName" };

            (this.mockProjectCloudTableClient, this.projectController) = UnitTestSetupHelper.GetProjectControllerAndCloudTableClientMock();
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
            var result = await this.projectController.GetProjectsAsync(this.getProjectWithSomeName) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.NotFound);
        }
    }
}