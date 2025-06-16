using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Infrastructure.Repositories;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Service
{
    public static class ModuleServiceDependencies
    {

        public static IServiceCollection AddModuleServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRequirementsService, RequirementsService>();



            return services;
        }
    }
}
