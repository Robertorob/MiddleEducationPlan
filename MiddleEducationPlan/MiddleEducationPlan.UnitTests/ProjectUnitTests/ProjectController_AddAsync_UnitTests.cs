using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using MiddleEducationPlan.UnitTests.Helpers;
using MiddleEducationPlan.Web.Controllers;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Project
{
    public class ProjectController_AddAsync_UnitTests
    {
        private ProjectController projectController;
        private AddProjectModel addProjectModel;
        private AddProjectModel addEmptyNameProjectModel;

        [SetUp]
        public void Setup()
        {
            this.addProjectModel = new AddProjectModel
            {
                Name = "project1"
            };
            this.addEmptyNameProjectModel = new AddProjectModel();

            this.projectController = UnitTestSetupHelper.GetProjectControllerMock();
        }

        [Test]
        public async Task AddAsync_ValidProject_Ok200()
        {
            var result = await this.projectController.AddAsync(this.addProjectModel) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int) HttpStatusCode.Created);
        }

        [Test]
        public async Task AddAsync_EmptyName_BadRequest()
        {
            var result = Validator.TryValidateObject(this.addEmptyNameProjectModel, new ValidationContext(this.addEmptyNameProjectModel, null, null), null, true);

            Assert.IsFalse(result);

            this.projectController.ModelState.AddModelError("Name", "The 'Name' field is required");
            var response = await this.projectController.AddAsync(this.addEmptyNameProjectModel) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.BadRequest);
        }
    }
}