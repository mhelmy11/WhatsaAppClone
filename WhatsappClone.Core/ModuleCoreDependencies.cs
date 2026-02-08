using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MoMediatoR;
using System.Reflection;
using WhatsappClone.Core.Behaviours;
using WhatsappClone.Core.Filters;
using WhatsappClone.Core.RequirementsHandlers;

namespace WhatsappClone.Core
{
    public static class ModuleCoreDependencies
    {

        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            // Register MediatR


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));


            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            services.AddTransient<SeesionNotRevokedRequirementHandler>();

            return services;
        }



    }
}
