using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
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
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMessagesService, MessagesService>();
            services.AddScoped<IMessageStatusesService, MessageStatusesService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IContactsService, ContactService>();




            return services;
        }
    }
}
