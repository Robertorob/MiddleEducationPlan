using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.BusinessLogic.Models.Task;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.BusinessLogic.Models.Project
{
    public class AddProjectModel
    {
        [Required]
        public string Name { get; set; }
        public IEnumerable<AddTaskModel> Tasks { get; set; }
    }
}
