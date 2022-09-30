using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DinnamuSWebApi.Controllers
{
    public class IndexController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }
    }
}
