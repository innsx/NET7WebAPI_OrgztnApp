using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;

namespace NET7WebAPI_OrgztnApp.API.Controllers
{

    //we specified this Route as we’ve created & specified in program.cs app.UseExceptionHandler(“/Errors”)
    [Route("/Errors")]
    //[Route("api/[controller]")]
    //[ApiController]
    public sealed class ErrorsController : ControllerBase
    {
        public IActionResult Error()
        {
            //In ASP.NET Core, this line is used to retrieve the original exception
            //that occurred during a request so it can be handled or logged

            //Purpose: This code is typically placed within a custom error handler action
            //or a lambda function passed to app.UseExceptionHandler()

            //Mechanism: When an unhandled exception occurs, the Exception Handler Middleware catches it
            //and stores the details in the HttpContext.Features collection
            //before re-executing the request pipeline for the error page.

            //IExceptionHandlerFeature: The base interface providing access to the Error property.

            //IExceptionHandlerPathFeature: Inherits from the base interface and adds a Path property,
            //which reveals the original URL where the error happened.
            //Use this if you need to log the specific route that failed.
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            //c#9.0 switch - case statements
            var (statusCode, message) = exception switch
            {
                IApplicationException appException => (Convert.ToInt32(appException.StatusCode), appException.ErrorMessage),

                //switch _ case is the DEFAULT statement
                _ => (StatusCodes.Status500InternalServerError, "An Unexpected error occurred")
            };


            //Accessing Details: Once you have the exception object,
            //you can access standard properties like Message and StackTrace
            return Problem(statusCode: statusCode, title: exception?.Message);
        }
    }
}
