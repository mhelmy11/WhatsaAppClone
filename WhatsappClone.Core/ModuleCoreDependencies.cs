using Microsoft.Extensions.DependencyInjection;
using MoMediatoR;
using System.Reflection;

namespace WhatsappClone.Core
{
    public static class ModuleCoreDependencies
    {

        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            // Register MediatR
            services.AddMoMediatoR(Assembly.GetExecutingAssembly());

            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }



    }
}
