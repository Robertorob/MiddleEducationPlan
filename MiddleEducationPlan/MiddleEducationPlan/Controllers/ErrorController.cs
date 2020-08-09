using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiddleEducationPlan.Web.Exceptions;
using MiddleEducationPlan.Web.Models;
using System.Net;

namespace MiddleEducationPlan.Web.Controllers
{
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;
            var code = HttpStatusCode.InternalServerError;

            if (exception is HttpStatusCodeException httpStatusCodeException)
            {
                code = httpStatusCodeException.Status;
            }

            Response.StatusCode = (int) code;

            return new ErrorResponse(exception);
        }
    }
}
