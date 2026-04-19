using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NET7WebAPI_OrgztnApp.API.Swagger;
using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Infrastructure.Persistences.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NET7WebAPI_OrgztnApp.API.Configurations
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddPresentationService(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddApiVersioning(options =>
            {
                //enable ApiVersions
                options.ReportApiVersions = true;

                //DEFAULT API version is SET to version 1
                options.DefaultApiVersion = new ApiVersion(1, 0);

                //take the 1st version, if NOT Specified a version at this line “options.DefaultApiVersion…”
                options.AssumeDefaultVersionWhenUnspecified = true;

                ////query string custom versioning
                //options.ApiVersionReader = new QueryStringApiVersionReader("organisationapptest-api-version");

                ////header versioning
                //options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
            });

            services.AddVersionedApiExplorer(options =>
            {
                //DEFAULT VALUE IS NULL
                //string format use to format API Version as a Group name
                options.GroupNameFormat = "'v'VVV";

                //DEFAULT VALUE IS FALSE
                //if set TRUE, the API Version parameters should be substitute in route templates
                //     [Route("api/v{v:apiVersion}/[controller]")]
                options.SubstituteApiVersionInUrl = true;
            });

            //REGISTER SWAGGERGETOPTIONS
            //WHEN setting up AddApiVersioning,
            //  we MUST REGISTER "<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()"
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();


            //injecting IUnitOfWork,UnitOfWork into the DI Container
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
