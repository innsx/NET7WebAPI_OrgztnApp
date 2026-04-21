using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace NET7WebAPI_OrgztnApp.API.Commons
{
    //ProblemDetailsFactory class is a Factory to create ProblemDetails and ValidationProblemDetails.
    //Using custom ProblemDetailsFactory along with exception middleware
    //we can easily tackle unhandled and custom exceptions
    public class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _apiBehaviorOptions;
        private readonly Action<ProblemDetailsContext>? _problemDetailsContextAction;

        //we are injecting IOptions<ApiBehaviorOptions> & IOptions<ProblemDetailsOptions>
        //  into this class constructor to access their settings at runtime: 
        //IOptions<T>: Registered as a Singleton; values are fixed for the application lifetime.
        public CustomProblemDetailsFactory(IOptions<ApiBehaviorOptions> apiBehaviorOptions, 
            IOptions<ProblemDetailsOptions>? problemDetailsOptions = null)
        {
            _apiBehaviorOptions = apiBehaviorOptions?.Value ?? 
                throw new ArgumentNullException(nameof(apiBehaviorOptions));

            _problemDetailsContextAction = problemDetailsOptions?.Value?.CustomizeProblemDetails;
        }


        //We needed to OVERRIDE ProblemDetails 
        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {

            //Null Conditional Assignment
            //if statusCode = null, then assign code: 500 as Internal server error
            statusCode ??= 500; 

            //creates ProblemDetails object
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            SettingUpProblemDetailsReturnFormat(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }


        //We needed to OVERRIDE ValidationProblemDetails
        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {

            ArgumentNullException.ThrowIfNull(modelStateDictionary);

            ////if statusCode = null, assign code: 400 as badrequest
            statusCode ??= 400;  

            //creates ValidationProblemDetails
            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            if (title != null)
            {
                // For validation problem details, don't overwrite the default title with null.
                problemDetails.Title = title;
            }

            SettingUpProblemDetailsReturnFormat(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }


        private void SettingUpProblemDetailsReturnFormat(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {
            problemDetails.Status ??= statusCode;

            if (_apiBehaviorOptions.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

            if (traceId != null)
            {
                problemDetails.Extensions["traceId"] = traceId;
            }

            //added customized error property
            problemDetails.Extensions.Add("CustomProperty", "Custom Property value");

            //creating an anonymous object & invoke this object
            _problemDetailsContextAction?.Invoke(
                new() 
                { 
                    HttpContext = httpContext!, 
                    ProblemDetails = problemDetails 
                }
            );
        }
    }
}
