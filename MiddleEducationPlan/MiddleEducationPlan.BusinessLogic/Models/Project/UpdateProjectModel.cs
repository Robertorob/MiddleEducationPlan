using System.ComponentModel.DataAnnotations;

namespace MiddleEducationPlan.BusinessLogic.Models.Project
{
    public class UpdateProjectModel
    {
        [Required]
        public string Name { get; set; }
    }
}
