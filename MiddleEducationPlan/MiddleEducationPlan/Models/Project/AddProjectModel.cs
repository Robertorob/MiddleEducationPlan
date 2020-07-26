﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Models
{
    public class AddProjectModel
    {
        public string Name { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}
