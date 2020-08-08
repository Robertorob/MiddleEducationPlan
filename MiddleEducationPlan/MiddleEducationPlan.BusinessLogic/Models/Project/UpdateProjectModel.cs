using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.BusinessLogic.Models.Project
{
    public class UpdateProjectModel
    {
        [Required]
        public string Name { get; set; }
    }
}
