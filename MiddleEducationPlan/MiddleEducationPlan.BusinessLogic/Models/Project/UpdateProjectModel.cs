using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiddleEducationPlan.BusinessLogic.Models.Project
{
    public class UpdateProjectModel
    {
        [Required]
        public string Name { get; set; }
        public ProjectType ProjectType { get; set; }
        public string Description { get; set; }
        public List<string> Owners { get; set; }
    }
}
