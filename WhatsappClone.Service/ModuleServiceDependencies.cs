using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Service
{
    public static class ModuleServiceDependencies
    {

        public static IServiceCollection AddModuleServiceDependencies(this IServiceCollection services , IConfiguration configuration)
        {
            
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRequirementsService, RequirementsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<PhoneNumberService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.Configure<SendGridSettings>(configuration.GetSection("SendGridSettings"));//then we can inject IOptions<SendGridSettings> to get the settings values

            var SendGridSettings = configuration.GetSection("SendGridSettings").Get<SendGridSettings>();



            return services;
        }
    }
}
