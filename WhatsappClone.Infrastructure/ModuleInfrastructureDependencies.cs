using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Infrastructure.Repositories;

namespace WhatsappClone.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {

        public static IServiceCollection AddModuleInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<ChatRepo>();
            services.AddScoped(typeof(IRepo<>), typeof(Repo<>));



            return services;
        }
    }
}
