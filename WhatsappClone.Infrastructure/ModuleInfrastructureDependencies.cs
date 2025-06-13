using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public static IServiceCollection AddModuleInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region CustomServices
            services.AddScoped<IChat, ChatRepo>();
            services.AddScoped(typeof(IRepo<>), typeof(Repo<>));

            #endregion

            #region Identity And DB
            services.AddDbContext<Context>(options =>
             {
                 options.UseSqlServer(configuration.GetConnectionString("whatsapp"));
             });

            services.AddIdentity<AppUser, IdentityRole>(

                options =>
                {
                    options.User.RequireUniqueEmail = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                }

                ).AddEntityFrameworkStores<Context>();

            #endregion


            return services;
        }
    }
}
