using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;
using IdGen;
using Microsoft.AspNetCore.Http;

namespace WhatsappClone.Core.Features.Identity.Commands
{
    public class RequestOtpCommandHandler : ResponseHandler, IRequestHandler<RequestOtpCommand, Response<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RequestOtpCommandHandler> _logger;
        private readonly IEmailService emailService;
        private readonly IIdGenerator<long> idGenerator;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RequestOtpCommandHandler(UserManager<User> userManager ,
            ILogger<RequestOtpCommandHandler> logger ,
            IEmailService emailService ,
            IIdGenerator<long> idGenerator 
            )
        {
            _userManager = userManager;
            _logger = logger;
            this.emailService = emailService;
            this.idGenerator = idGenerator;
        }
        public async Task<Response<string>> Handle(RequestOtpCommand request, CancellationToken ct)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) //new user
            {
                user = new User()
                {
                    Id = idGenerator.CreateId(),
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.PhoneNumber,
                    CountryCode = request.CountryCode,
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {

                    return BadRequest<string>("Registeration Failed");

                }
            }
            //generate otp
            var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            //send it via email service
            await emailService.SendEmailAsync(request.Email , "OTP Verification" , $"Your code is {otp}");

            return Success("OTP Send Successfully ");
        }
    }
}
