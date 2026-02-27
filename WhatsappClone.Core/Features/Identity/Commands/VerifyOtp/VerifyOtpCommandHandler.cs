using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Identity.Commands
{
    public class VerifyOtpCommandHandler : ResponseHandler,
        IRequestHandler<VerifyOtpCommand, Response<VerifyOtpResult>>
    {
        private readonly UserManager<User> userManager;
        private readonly IAuthenticationService authenticationService;

        public VerifyOtpCommandHandler(UserManager<User> userManager , IAuthenticationService authenticationService)
        {
            this.userManager = userManager;
            this.authenticationService = authenticationService;
        }
        public async Task<Response<VerifyOtpResult>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            var result =  await userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Otp);
            if (result)
            {
                var tokens = await authenticationService.GetTokenAfterLogin(user);
                return Success(new VerifyOtpResult { tokens = tokens});
            }
            return BadRequest<VerifyOtpResult>();

           
            
        }
    }
}
