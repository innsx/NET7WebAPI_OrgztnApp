using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NET7WebAPI_OrgztnApp.API.Configurations;
using NET7WebAPI_OrgztnApp.Application.Configurations;
using NET7WebAPI_OrgztnApp.Infrastructure.Configurations;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Register services
builder.Services
    .AddPresentationService()
    .AddApplicationService()
    .AddInfrastructureService();


var app = builder.Build();

//registering IApiVersionDescriptionProvider we injected in ConfigureSwaggerOptions class
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    //app.UseSwaggerUI();
    //adding more Swagger UI Options
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", 
                description.GroupName.ToUpperInvariant()
            );

            //options.RoutePrefix = "api/documentation"; //Adds a PREFIX to SWAGGER's ROUTE
            options.DefaultModelExpandDepth(2);
            options.DocExpansion(DocExpansion.List);  //Options: List(set as DEFAULT), Full, None
            options.DisplayRequestDuration();
        }
    });

}

// we will add this line as a middleware PIPELINE
// & every time an ERROR occurred,  
// ErrorsController.cs class’s Error( ) Endpoint will get HITTED
// & the Error( ) will catch ALL EXCEPTIONS & LOGGED THOSE EXCEPTIONS
app.UseExceptionHandler("/Errors");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
