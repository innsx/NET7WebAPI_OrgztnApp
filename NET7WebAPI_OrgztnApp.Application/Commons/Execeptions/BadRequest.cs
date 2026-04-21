using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NET7WebAPI_OrgztnApp.Application.Commons.Execeptions
{
    public class BadRequest : Exception, IApplicationException
    {
        public BadRequest(string errorMessage) : base(errorMessage)
        {
            
        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
