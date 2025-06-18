using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly SendGridSettings _settings;
        public EmailService(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value;

        }

        public async Task<string> SendEmailAsync(string toEmail, string subject, string htmlContent)
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

            Console.WriteLine(errorBody);

            if (!response.IsSuccessStatusCode)
            {

                throw new Exception($"Failed to send email: {response.StatusCode} - {errorBody}");


            }

            return "Email sent successfully.";
        }
    }
}
