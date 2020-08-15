using System.ComponentModel.DataAnnotations;

namespace MiddleEducationPlan.BusinessLogic.Models.Project
{
    public class AddProjectModel
    {
        [Required]
        public string Name { get; set; }
    }
}
