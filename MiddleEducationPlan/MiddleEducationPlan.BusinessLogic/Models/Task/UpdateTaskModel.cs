using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.BusinessLogic.Models.Task
{
    public class UpdateTaskModel
    {
        [Required]
        public Guid? ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
