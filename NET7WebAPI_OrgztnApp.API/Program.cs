using NET7WebAPI_OrgztnApp.API.Configurations;
using NET7WebAPI_OrgztnApp.Application.Configurations;
using NET7WebAPI_OrgztnApp.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Register services
builder.Services
    .AddPresentationService()
    .AddApplicationService()
    .AddInfrastructureService();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
