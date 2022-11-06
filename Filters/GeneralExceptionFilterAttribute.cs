using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DinnamuSWebApi.Filters
{
    public class GeneralExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
         var ex = context.Exception?.InnerException?.InnerException as Exception;
         context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            Debug.Print(ex.Message);
            //if (!(context.Exception is Exception)) return;   
        }
    }
}