using System;

namespace MiddleEducationPlan.BusinessLogic.Models.Task
{
    public class GetTaskModel
    {
        public Guid? ProjectId { get; set; }
        public string Name { get; set; }
    }
}
