using IdGen;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
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
    public class VerifyOtpCommandHandler : ResponseHandler,
        IRequestHandler<VerifyOtpCommand, Response<VerifyOtpResult>>
    {
        private readonly UserManager<User> userManager;
        private readonly IAuthenticationService authenticationService;
        private readonly IMemoryCache memoryCache;
        private readonly PhoneNumberService phoneNumberService;
        private readonly IIdGenerator<long> idGenerator;

        public VerifyOtpCommandHandler(UserManager<User> userManager ,
            IAuthenticationService authenticationService ,
            IMemoryCache memoryCache ,
            PhoneNumberService phoneNumberService,
            IIdGenerator<long> idGenerator
            )
        {
            this.userManager = userManager;
            this.authenticationService = authenticationService;
            this.memoryCache = memoryCache;
            this.phoneNumberService = phoneNumberService;
            this.idGenerator = idGenerator;
            this.idGenerator = idGenerator;
        }
        public async Task<Response<VerifyOtpResult>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            if (!memoryCache.TryGetValue($"{request.Email}", out string? savedOtp) || savedOtp != request.Otp)
            {
                return BadRequest<VerifyOtpResult>("Verification failed");
            }
            memoryCache.Remove($"{request.Email}");

  
            var user = await userManager.FindByEmailAsync(request.Email);

            var (cleanCountryCode, cleanNationalNumber) = phoneNumberService.CleanPhoneNumber(request.CountryCode, request.PhoneNumber);

            if (user == null)
            {
                user = new User
                {
                    Id = idGenerator.CreateId(),
                    Email = request.Email,
                    PhoneNumber = cleanNationalNumber,
                    UserName = cleanNationalNumber,
                    CountryCode = cleanCountryCode,
                };

               var result =  await userManager.CreateAsync(user);
               if (!result.Succeeded)
               {
                    return BadRequest<VerifyOtpResult>("Registeration Failed");
               }

            }
            var tokens = await authenticationService.GetTokenAfterLogin(user);
            return Success(new VerifyOtpResult { tokens = tokens , Name = user.Name , ProfilePic = user.ProfilePicUrl},"Verified");

           
            
        }
    }
}
