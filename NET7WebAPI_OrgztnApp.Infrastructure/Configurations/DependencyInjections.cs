using Microsoft.Extensions.DependencyInjection;
using NET7WebAPI_OrgztnApp.Infrastructure.Persistences.DataContexts;

namespace NET7WebAPI_OrgztnApp.Infrastructure.Configurations
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            //if a service is a CLASS,
            //then we register the service using 
            //1 of 3 service types’ LIFETIME: AddScoped(), AddTransient(), AddSingleton( )
            services.AddScoped<DapperDataContext>();

            return services;
        }
    }
}
