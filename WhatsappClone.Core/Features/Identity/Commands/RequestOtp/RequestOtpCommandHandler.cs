using IdGen;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Identity.Commands
{
    public class RequestOtpCommandHandler : ResponseHandler, IRequestHandler<RequestOtpCommand, Response<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RequestOtpCommandHandler> _logger;
        private readonly INotificationFactoryService notificationFactory;
        private readonly IIdGenerator<long> idGenerator;
        private readonly PhoneNumberService phoneNumberService;
        private readonly IMemoryCache memoryCache;

        public RequestOtpCommandHandler(UserManager<User> userManager,
            ILogger<RequestOtpCommandHandler> logger,
            INotificationFactoryService notificationFactory ,
            IIdGenerator<long> idGenerator,
            PhoneNumberService phoneNumberService,
            IMemoryCache memoryCache
            )
        {
            _userManager = userManager;
            _logger = logger;
            this.notificationFactory = notificationFactory;
            this.idGenerator = idGenerator;
            this.phoneNumberService = phoneNumberService;
            this.memoryCache = memoryCache;
        }
        public async Task<Response<string>> Handle(RequestOtpCommand request, CancellationToken ct)
        {

            //generate otp
            string otp = new Random().Next(100000, 999999).ToString();
            memoryCache.Set($"{request.Email}", otp, TimeSpan.FromMinutes(5));


            var notificationTarget = notificationFactory.GetService(NotificationType.Email);

            //send it via email service
            var status = await notificationTarget.SendMessageAsync(request.Email, "OTP Verification", $"Your code is {otp}");

            _logger.Log(LogLevel.Information, $"Your code is {otp}");
           return status ?  Success("OTP Send Successfully") : BadRequest<string>("An error occurred while sending the email");
        }
    }
}
