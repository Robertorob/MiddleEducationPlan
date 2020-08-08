using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.BusinessLogic.Models.Task
{
    public class GetTaskModel
    {
        public Guid? ProjectId { get; set; }
        public string Name { get; set; }
    }
}
