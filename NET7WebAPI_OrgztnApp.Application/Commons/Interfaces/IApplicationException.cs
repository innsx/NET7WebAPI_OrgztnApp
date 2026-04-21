using System.Net;

namespace NET7WebAPI_OrgztnApp.Application.Commons.Interfaces
{
    public interface IApplicationException
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorMessage { get; }
    }
}
