using System;
using System.ComponentModel.DataAnnotations;

namespace MiddleEducationPlan.BusinessLogic.Models.Task
{
    public class AddTaskModel
    {
        [Required]
        public Guid? ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
