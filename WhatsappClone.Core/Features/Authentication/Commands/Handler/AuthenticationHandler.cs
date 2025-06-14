using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Authentication.Commands.Models;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Authentication.Commands.Handler
{
    public class AuthenticationHandler : ResponseHandler, IRequestHandler<LoginCommand, Response<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IAuthenticationService authenticationService;

        public AuthenticationHandler(UserManager<AppUser> userManager, IAuthenticationService authenticationService)
        {
            this.userManager = userManager;
            this.authenticationService = authenticationService;
        }

        public async Task<Response<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = userManager.FindByPhoneNumber(request.PhoneNumber);
            if (user == null) return BadRequest<string>("User not found. Please register first.");
            var isValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isValid) return BadRequest<string>("Invalid password. Please try again.");
            var token = await authenticationService.GetToken(user);
            if (string.IsNullOrEmpty(token)) return BadRequest<string>("Token generation failed. Please try again.");

            return Success<string>(token, "Login successful. Token generated successfully.");


        }
    }
}
