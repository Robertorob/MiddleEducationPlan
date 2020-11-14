using System.Collections.Generic;

namespace MiddleEducationPlan.BusinessLogic.Models.Project
{
    public class GetProjectModel
    {
        public string Name { get; set; }
        public ProjectType ProjectType { get; set; }
        public string Description { get; set; }
        public bool IncludeTasks { get; set; }
    }
}
