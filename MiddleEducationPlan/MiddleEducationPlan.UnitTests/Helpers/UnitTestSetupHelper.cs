using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Services;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.UnitTests.Project.Mock;
using MiddleEducationPlan.UnitTests.TaskUnitTests.Mock;
using MiddleEducationPlan.Web.Controllers;
using Moq;
using System;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Helpers
{
    public static class UnitTestSetupHelper
    {
        private const string BASE_ADDRESS = "https://educationplanstorageacc.table.core.windows.net/";

        public static (MockProjectCloudTableClient, ProjectController) GetProjectControllerAndCloudTableClientMock()
        {
            var taskServiceMock = new Mock<ITaskService>();

            var mockProjectCloudTableClient = new MockProjectCloudTableClient(new Uri(UnitTestSetupHelper.BASE_ADDRESS),
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials());

            var cloudTableClientFactoryMock = new Mock<ICloudTableClientFactory>();
            cloudTableClientFactoryMock.Setup(f => f.GetCloudTableClient()).Returns(mockProjectCloudTableClient);

            var projectService = new ProjectService(taskServiceMock.Object, cloudTableClientFactoryMock.Object);

            return (mockProjectCloudTableClient, new ProjectController(projectService));
        }

        public static (MockTaskCloudTableClient, TaskController) GetTaskControllerAndCloudTableClientMock()
        {
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(f => f.GetProjectByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new BusinessLogic.TableEntities.ProjectEntity()));

            var mockTaskCloudTableClient = new MockTaskCloudTableClient(new Uri(UnitTestSetupHelper.BASE_ADDRESS),
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials());

            var cloudTableClientFactoryMock = new Mock<ICloudTableClientFactory>();
            cloudTableClientFactoryMock.Setup(f => f.GetCloudTableClient()).Returns(mockTaskCloudTableClient);

            var taskService = new TaskService(cloudTableClientFactoryMock.Object);

            return (mockTaskCloudTableClient, new TaskController(taskService, projectServiceMock.Object));
        }
    }
}
