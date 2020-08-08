using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace MiddleEducationPlan.BusinessLogic.TableEntities
{
    public class TaskEntity : TableEntity
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
