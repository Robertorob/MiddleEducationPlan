using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Web.Models
{
    public class ResultModel<T>
    {
        public Status Status { get; set; }
        public T Value { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum Status { Success = 0, Error = 1 }
}
