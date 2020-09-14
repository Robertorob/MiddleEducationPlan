using System.Collections.Generic;

namespace MiddleEducationPlan.Web.Models
{
    public class SelectModel<T>
    {
        public List<T> Values { get; set; }
        public int DefaultValue { get; set; }
    }
}
