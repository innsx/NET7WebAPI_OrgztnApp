using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using System.Net;

namespace NET7WebAPI_OrgztnApp.Application.Commons.Execeptions
{
    public class DuplicateNameException : Exception, IApplicationException
    {
        public DuplicateNameException(string errorMessage) : base(errorMessage) 
        { 
        }

        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        //public HttpStatusCode StatusCode
        //{
        //    get
        //    {
        //        return HttpStatusCode.Conflict;
        //    }
        //}

        public string ErrorMessage => Message;
        //public string ErrorMessage
        //{
        //    get
        //    {
        //        return ErrorMessage;
        //    }
        //}
    }
}
