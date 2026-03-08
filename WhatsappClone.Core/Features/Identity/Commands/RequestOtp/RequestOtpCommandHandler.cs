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
        private readonly IEmailService emailService;
        private readonly IIdGenerator<long> idGenerator;
        private readonly PhoneNumberService phoneNumberService;
        private readonly IMemoryCache memoryCache;

        public RequestOtpCommandHandler(UserManager<User> userManager,
            ILogger<RequestOtpCommandHandler> logger,
            IEmailService emailService,
            IIdGenerator<long> idGenerator,
            PhoneNumberService phoneNumberService,
            IMemoryCache memoryCache
            )
        {
            _userManager = userManager;
            _logger = logger;
            this.emailService = emailService;
            this.idGenerator = idGenerator;
            this.phoneNumberService = phoneNumberService;
            this.memoryCache = memoryCache;
        }
        public async Task<Response<string>> Handle(RequestOtpCommand request, CancellationToken ct)
        {

            //generate otp
            string otp = new Random().Next(100000, 999999).ToString();
            memoryCache.Set($"{request.Email}", otp, TimeSpan.FromMinutes(5));


            //send it via email service
            await emailService.SendEmailAsync(request.Email, "OTP Verification", $"Your code is {otp}");

            return Success("OTP Send Successfully");
        }
    }
}
