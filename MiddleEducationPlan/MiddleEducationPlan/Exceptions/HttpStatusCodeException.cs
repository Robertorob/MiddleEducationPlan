using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiddleEducationPlan.Web.Exceptions
{
    public class HttpStatusCodeException : Exception
    {
        public HttpStatusCode Status { get; private set; }

        public HttpStatusCodeException(HttpStatusCode statusCode, string msg) : base(msg)
        {
            Status = statusCode;
        }
    }
}
