using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Services;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.UnitTests.Project.Mock;
using MiddleEducationPlan.Web.Controllers;
using Moq;
using System;

namespace MiddleEducationPlan.UnitTests.Helpers
{
    public static class UnitTestSetupHelper
    {
        public static (MockProjectCloudTableClient, ProjectController) GetProjectControllerAndCloudTableClientMock()
        {
            var taskServiceMock = new Mock<ITaskService>();

            var mockProjectCloudTableClient = new MockProjectCloudTableClient(new Uri("https://educationplanstorageacc.table.core.windows.net/"),
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials());

            var cloudTableClientFactoryMock = new Mock<ICloudTableClientFactory>();
            cloudTableClientFactoryMock.Setup(f => f.GetCloudTableClient()).Returns(mockProjectCloudTableClient);

            var projectService = new ProjectService(taskServiceMock.Object, cloudTableClientFactoryMock.Object);

            return (mockProjectCloudTableClient, new ProjectController(projectService));
        }
    }
}
