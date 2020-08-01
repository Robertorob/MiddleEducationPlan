using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Models.Project
{
    public class UpdateProjectModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}
