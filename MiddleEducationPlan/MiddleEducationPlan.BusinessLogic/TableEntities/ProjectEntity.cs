using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.Models.Project;
using System;
using System.Collections.Generic;

namespace MiddleEducationPlan.BusinessLogic.TableEntities
{
    public class ProjectEntity : TableEntity
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int ProjectTypeInteger { get; set; }
        public string Description { get; set; }
        public string Owners { get; set; }
        public IEnumerable<TaskEntity> Tasks { get; set; }
    }
}
