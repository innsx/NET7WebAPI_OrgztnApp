using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using System.Net;

namespace NET7WebAPI_OrgztnApp.Application.Commons.Execeptions
{
    public class NotFoundException : Exception, IApplicationException
    {
        public NotFoundException(string errorMessage) : base(errorMessage)
        {

        }

        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;

    }
}
