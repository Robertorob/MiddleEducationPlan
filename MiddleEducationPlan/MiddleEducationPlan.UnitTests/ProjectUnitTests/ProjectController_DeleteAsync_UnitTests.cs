using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.UnitTests.Helpers;
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

        [SetUp]
        public void Setup()
        {
            this.projectController = UnitTestSetupHelper.GetProjectControllerMock();
        }

        [Test]
        public async Task DeleteAsync_ExistingProjects_Ok200()
        {
            var result = await this.projectController.DeleteAsync(UnitTestSetupHelper.ExistingProjects[0].Id) as ObjectResult;

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