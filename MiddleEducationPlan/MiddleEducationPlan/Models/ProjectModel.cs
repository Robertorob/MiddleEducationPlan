using System;

namespace MiddleEducationPlan.Web.Models
{
    public class ProjectModel
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string ProjectType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
