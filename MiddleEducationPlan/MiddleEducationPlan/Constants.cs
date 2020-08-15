using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Web
{
    public static class Constants
    {
#if RELEASE
        public const string BaseAddress = "https://middleeducationplan.azurewebsites.net/api";
#else
        public const string BaseAddress = "https://localhost:44332/api";
#endif
        public static readonly string ProjectBaseAddress = $"{BaseAddress}/project";
        public static readonly string TaskBaseAddress = $"{BaseAddress}/task";
    }
}
