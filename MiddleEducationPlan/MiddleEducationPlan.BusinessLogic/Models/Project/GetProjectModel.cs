using System.Collections.Generic;

namespace MiddleEducationPlan.BusinessLogic.Models.Project
{
    public class GetProjectModel
    {
        public int? Code { get; set; }
        public string Name { get; set; }
        public ProjectType ProjectType { get; set; }
        public string Description { get; set; }
        public List<string> Owners { get; set; }
        public bool IncludeTasks { get; set; }
    }
}
