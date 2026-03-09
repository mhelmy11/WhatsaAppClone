using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class EmailService : INotificationService
    {
        private readonly SendGridSettings _settings;
        public EmailService(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value;

        }

        public async Task<bool> SendMessageAsync(string toEmail, string htmlContent, string? subject = null)
        {
            var apiKey = _settings.ApiKey;
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("SendGrid API Key is not configured.");
            }

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

            var response = await client.SendEmailAsync(msg);
            var errorBody = await response.Body.ReadAsStringAsync();


            if (!response.IsSuccessStatusCode)
            {

                throw new Exception($"Failed to send email: {response.StatusCode} - {errorBody}");


            }


            return true;

        }

        
    }
}
