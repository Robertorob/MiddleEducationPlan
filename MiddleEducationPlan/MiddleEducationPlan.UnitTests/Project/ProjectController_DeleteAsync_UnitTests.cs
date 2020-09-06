using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.UnitTests.Helpers;
using MiddleEducationPlan.UnitTests.Project.Mock;
using MiddleEducationPlan.Web.Controllers;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Project
{
    public class ProjectController_DeleteAsync_UnitTests
    {
        private ProjectController projectController;
        private MockProjectCloudTableClient mockProjectCloudTableClient;

        [SetUp]
        public void Setup()
        {
            (this.mockProjectCloudTableClient, this.projectController) = UnitTestSetupHelper.GetProjectControllerAndCloudTableClientMock();
        }

        [Test]
        public async Task DeleteAsync_ExistingProjects_Ok200()
        {
            var startCount = this.mockProjectCloudTableClient.mockProjectCloudTable.projects.Count;

            var result = await this.projectController.DeleteAsync(this.mockProjectCloudTableClient.mockProjectCloudTable.projects[1].Id) as ObjectResult;

            var endCount = this.mockProjectCloudTableClient.mockProjectCloudTable.projects.Count;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
        }

        [Test]
        public async Task DeleteAsync_NonExistingProject_NotFound404()
        {
            var result = await this.projectController.DeleteAsync(Guid.NewGuid()) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}