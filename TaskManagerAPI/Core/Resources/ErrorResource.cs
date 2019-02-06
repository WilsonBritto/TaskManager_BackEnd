using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TaskManagerAPI.Core.Resources
{
    public class ErrorResource
    {
        public HttpStatusCode errorCode { get; set; }
        public string error { get; set; }
    }
}