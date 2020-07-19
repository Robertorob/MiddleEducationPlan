using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Models
{
    public class Project : TableEntity
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string EntityName { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}
