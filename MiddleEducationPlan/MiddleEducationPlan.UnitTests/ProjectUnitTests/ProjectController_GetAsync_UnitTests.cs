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
    public class ProjectController_GetAsync_UnitTests
    {
        private ProjectController projectController;
        private MockProjectCloudTableClient mockProjectCloudTableClient;

        [SetUp]
        public void Setup()
        {
            (this.mockProjectCloudTableClient, this.projectController) = UnitTestSetupHelper.GetProjectControllerAndCloudTableClientMock();
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