using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.BusinessLogic.Services;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.UnitTests.Helpers;
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
    public class ProjectController_UpdateAsync_UnitTests
    {
        private ProjectController projectController;
        private UpdateProjectModel updateProjectModel;
        private MockProjectCloudTableClient mockProjectCloudTableClient;

        [SetUp]
        public void Setup()
        {
            this.updateProjectModel = new UpdateProjectModel
            {
                Name = "Updated name"
            };

            (this.mockProjectCloudTableClient, this.projectController) = UnitTestSetupHelper.GetProjectControllerAndCloudTableClientMock();
        }

        [Test]
        public async Task UpdateAsync_ExistingProjects_Ok200()
        {
            var result = await this.projectController.UpdateAsync(this.mockProjectCloudTableClient.mockProjectCloudTable.projects[1].Id, this.updateProjectModel) as ObjectResult;

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