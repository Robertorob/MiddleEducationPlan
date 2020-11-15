using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.UnitTests.Helpers;
using MiddleEducationPlan.Web.Controllers;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Project
{
    public class ProjectController_UpdateAsync_UnitTests
    {
        private ProjectController projectController;
        private UpdateProjectModel updateProjectModel;

        [SetUp]
        public void Setup()
        {
            this.updateProjectModel = new UpdateProjectModel
            {
                Name = "update"
            };

            this.projectController = UnitTestSetupHelper.GetProjectControllerMock();
        }

        [Test]
        public async Task UpdateAsync_ExistingProjects_Ok200()
        {
            var result = await this.projectController.UpdateAsync(UnitTestSetupHelper.ExistingProjects[0].Id, this.updateProjectModel) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.OK);
            Assert.AreEqual(((ProjectEntity)result.Value).Name, this.updateProjectModel.Name);
        }

        [Test]
        public async Task UpdateAsync_NonExistingProject_NotFound404()
        {
            var result = await this.projectController.UpdateAsync(Guid.NewGuid(), this.updateProjectModel) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}