using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace MiddleEducationPlan.BusinessLogic.TableEntities
{
    public class ProjectEntity : TableEntity
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
