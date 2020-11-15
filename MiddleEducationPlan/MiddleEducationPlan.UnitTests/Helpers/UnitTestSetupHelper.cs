using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.Interfaces;
using MiddleEducationPlan.BusinessLogic.Services;
using MiddleEducationPlan.BusinessLogic.TableEntities;
using MiddleEducationPlan.Common.Interfaces;
using MiddleEducationPlan.Common.Services;
using MiddleEducationPlan.Web.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MiddleEducationPlan.UnitTests.Helpers
{
    public static class UnitTestSetupHelper
    {
        private const string BASE_ADDRESS = "https://educationplanstorageacc.table.core.windows.net/";

        public static List<ProjectEntity> ExistingProjects;

        public static List<TaskEntity> ExistingTasks;

        static UnitTestSetupHelper()
        {
            ExistingProjects = new List<ProjectEntity>
            {
                new ProjectEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "project1",
                    Description = "description1",
                    ProjectTypeInteger = 1
                },
                new ProjectEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "project2",
                    Description = "description2",
                    ProjectTypeInteger = 2
                },
                new ProjectEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "project3",
                    Description = "description3",
                    ProjectTypeInteger = 2
                }
            };

            ExistingTasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "task1",
                    Description = "description1",
                    ProjectId = ExistingProjects[0].Id
                },
                new TaskEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "task2",
                    Description = "description2",
                    ProjectId = ExistingProjects[0].Id
                },
                new TaskEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "task3",
                    Description = "description3",
                    ProjectId = ExistingProjects[1].Id
                }
            };
        }

        public static ProjectController GetProjectControllerMock()
        {
            var taskServiceMock = new Mock<ITaskService>();

            var storageAccountServieMock = new Mock<IStorageAccountService<ProjectEntity>>();
            storageAccountServieMock.Setup(f => f.GetEntityByIdAsync(ExistingProjects[0].Id)).Returns(Task.FromResult(ExistingProjects[0]));
            storageAccountServieMock.Setup(f => f.GetEntityByIdAsync(ExistingProjects[1].Id)).Returns(Task.FromResult(ExistingProjects[1]));
            storageAccountServieMock.Setup(f => f.GetEntityByIdAsync(ExistingProjects[2].Id)).Returns(Task.FromResult(ExistingProjects[2]));

            storageAccountServieMock.Setup(f => f.AddEntityAsync(It.Is<ProjectEntity>(b => b.Name == ExistingProjects[0].Name))).Returns(Task.FromResult(new TableResult
            {
                Result = ExistingProjects[0]
            }));
            storageAccountServieMock.Setup(f => f.AddEntityAsync(It.Is<ProjectEntity>(b => b.Name == ExistingProjects[1].Name))).Returns(Task.FromResult(new TableResult
            {
                Result = ExistingProjects[1]
            }));
            storageAccountServieMock.Setup(f => f.AddEntityAsync(It.Is<ProjectEntity>(b => b.Name == ExistingProjects[2].Name))).Returns(Task.FromResult(new TableResult
            {
                Result = ExistingProjects[2]
            }));

            storageAccountServieMock.Setup(f => f.DeleteEntityByIdAsync(It.Is<Guid>(b => b == ExistingProjects[0].Id))).Returns(Task.FromResult(new TableResult
            {
                Result = ExistingProjects[0]
            }));

            var ctor = typeof(TableQuerySegment<ProjectEntity>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            var emptyQuerySegment = ctor.Invoke(new object[] { new List<ProjectEntity>() }) as TableQuerySegment<ProjectEntity>;

            var allProjectsQuerySegment = ctor.Invoke(new object[] { ExistingProjects }) as TableQuerySegment<ProjectEntity>;

            storageAccountServieMock.Setup(f => f.ExecuteQuerySegmentedAsync(It.Is<TableQuery<ProjectEntity>>(b => b.FilterString == "Name eq 'nonExistingName'"))).Returns(Task.FromResult(emptyQuerySegment));

            storageAccountServieMock.Setup(f => f.ExecuteQuerySegmentedAsync(It.Is<TableQuery<ProjectEntity>>(b => string.IsNullOrEmpty(b.FilterString)))).Returns(Task.FromResult(allProjectsQuerySegment));

            storageAccountServieMock.Setup(f => f.UpdateEntityAsync(It.Is<ProjectEntity>(b => b.Id == ExistingProjects[0].Id))).Returns(Task.FromResult(new TableResult 
            {
                Result = new ProjectEntity
                {
                    Id = ExistingProjects[0].Id,
                    Name = "update"
                }
            }));
            //storageAccountServieMock.Setup(f => f.AddEntityAsync(ExistingProjects[1])).Returns(Task.FromResult(new Microsoft.WindowsAzure.Storage.Table.TableResult()));
            //storageAccountServieMock.Setup(f => f.AddEntityAsync(ExistingProjects[2])).Returns(Task.FromResult(new Microsoft.WindowsAzure.Storage.Table.TableResult()));

            var projectService = new ProjectService(taskServiceMock.Object, storageAccountServieMock.Object);

            return new ProjectController(projectService);
        }

        public static TaskController GetTaskControllerMock()
        {
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(f => f.GetProjectByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new ProjectEntity()));

            var storageAccountServieMock = new Mock<IStorageAccountService<TaskEntity>>();

            storageAccountServieMock.Setup(f => f.GetEntityByIdAsync(ExistingTasks[0].Id)).Returns(Task.FromResult(ExistingTasks[0]));
            storageAccountServieMock.Setup(f => f.GetEntityByIdAsync(ExistingTasks[1].Id)).Returns(Task.FromResult(ExistingTasks[1]));
            storageAccountServieMock.Setup(f => f.GetEntityByIdAsync(ExistingTasks[2].Id)).Returns(Task.FromResult(ExistingTasks[2]));

            storageAccountServieMock.Setup(f => f.AddEntityAsync(It.Is<TaskEntity>(b => b.Name == ExistingTasks[0].Name))).Returns(Task.FromResult(new TableResult
            {
                Result = ExistingTasks[0]
            }));
            storageAccountServieMock.Setup(f => f.AddEntityAsync(It.Is<TaskEntity>(b => b.Name == ExistingTasks[1].Name))).Returns(Task.FromResult(new TableResult
            {
                Result = ExistingTasks[1]
            }));
            storageAccountServieMock.Setup(f => f.AddEntityAsync(It.Is<TaskEntity>(b => b.Name == ExistingTasks[2].Name))).Returns(Task.FromResult(new TableResult
            {
                Result = ExistingTasks[2]
            }));

            storageAccountServieMock.Setup(f => f.DeleteEntityByIdAsync(It.Is<Guid>(b => b == ExistingTasks[0].Id))).Returns(Task.FromResult(new TableResult
            {
                Result = ExistingTasks[0]
            }));



            var ctor = typeof(TableQuerySegment<TaskEntity>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            var emptyQuerySegment = ctor.Invoke(new object[] { new List<TaskEntity>() }) as TableQuerySegment<TaskEntity>;

            var allTasksQuerySegment = ctor.Invoke(new object[] { ExistingTasks }) as TableQuerySegment<TaskEntity>;

            storageAccountServieMock.Setup(f => f.ExecuteQuerySegmentedAsync(It.Is<TableQuery<TaskEntity>>(b => b.FilterString == "Name eq 'nonExistingName'"))).Returns(Task.FromResult(emptyQuerySegment));

            storageAccountServieMock.Setup(f => f.ExecuteQuerySegmentedAsync(It.Is<TableQuery<TaskEntity>>(b => string.IsNullOrEmpty(b.FilterString)))).Returns(Task.FromResult(allTasksQuerySegment));

            storageAccountServieMock.Setup(f => f.UpdateEntityAsync(It.Is<TaskEntity>(b => b.Id == ExistingTasks[0].Id))).Returns(Task.FromResult(new TableResult
            {
                Result = new TaskEntity
                {
                    Id = ExistingTasks[0].Id,
                    Name = "update"
                }
            }));


            var taskService = new TaskService(storageAccountServieMock.Object);

            //var taskServiceMock = new Mock<ITaskService>();
            //taskServiceMock.Setup(f => f.GetTaskByIdAsync(ExistingTasks[0].Id)).Returns(Task.FromResult(ExistingTasks[0]));
            //taskServiceMock.Setup(f => f.GetTaskByIdAsync(ExistingTasks[1].Id)).Returns(Task.FromResult(ExistingTasks[1]));
            //taskServiceMock.Setup(f => f.GetTaskByIdAsync(ExistingTasks[2].Id)).Returns(Task.FromResult(ExistingTasks[2]));

            return new TaskController(taskService, projectServiceMock.Object);
        }
    }
}
