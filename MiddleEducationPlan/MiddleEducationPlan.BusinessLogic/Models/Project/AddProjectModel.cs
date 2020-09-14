using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiddleEducationPlan.BusinessLogic.Models.Project
{
    public class AddProjectModel
    {
        [Required]
        public string Name { get; set; }
        public ProjectType? ProjectType { get; set; }
        public string Description { get; set; }
        public List<string> Owners { get; set; } = new List<string>();
    }

    public enum ProjectType { None = 0, Normal = 1, Extreme = 2 }
}
