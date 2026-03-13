using FluentValidation;
using Hangfire;
using IdGen.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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

        public static IServiceCollection AddCoreDependencies(this IServiceCollection services , IConfiguration configuration)
        {
            // Register MediatR


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddIdGen(1);

            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            services.AddTransient<SeesionNotRevokedRequirementHandler>();
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(5); // OTP is revoked after 5 min
            });
            services.AddHangfire(config =>
                config.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();

            return services;
        }



    }
}
