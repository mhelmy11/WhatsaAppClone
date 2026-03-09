using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Service.Abstract
{
    public interface INotificationService
    {

        public Task<bool> SendMessageAsync(string destination, string message , string? emailSubject = null);
    }

    public enum NotificationType
    {
        Email,
        SMS,
        PushNotification
    }
}
