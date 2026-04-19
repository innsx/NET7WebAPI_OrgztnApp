using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Infrastructure.Persistences.Repositories;

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

            //injecting IUnitOfWork,UnitOfWork into the DI Container
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
