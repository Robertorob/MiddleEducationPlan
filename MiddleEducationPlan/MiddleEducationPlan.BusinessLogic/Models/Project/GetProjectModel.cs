using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.BusinessLogic.Models.Project
{
    public class GetProjectModel
    {
        public int? Code { get; set; }
        public string Name { get; set; }
        public bool IncludeTasks { get; set; }
    }
}
