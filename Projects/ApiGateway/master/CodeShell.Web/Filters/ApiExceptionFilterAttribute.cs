using CodeShell.Data.Helpers;
using CodeShell.Tracer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace CodeShell.Web.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            SubmitResult res = new SubmitResult(1,"Error");
            res.SetException(actionExecutedContext.Exception);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, res);
            Logger.WriteException(actionExecutedContext.Exception);
        }
    }
}
