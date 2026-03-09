using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class NotificationFactoryService : INotificationFactoryService
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationFactoryService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public INotificationService GetService(NotificationType type)
        {
            return _serviceProvider.GetRequiredKeyedService<INotificationService>(type);
        }
    }
}
