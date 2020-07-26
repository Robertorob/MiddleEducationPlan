using Microsoft.WindowsAzure.Storage.Table;
using MiddleEducationPlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.TableEntities
{
    public class ProjectEntity : TableEntity
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
