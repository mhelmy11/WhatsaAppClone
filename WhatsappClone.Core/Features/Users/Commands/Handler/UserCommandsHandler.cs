using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Users.Commands.Models;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Users.Commands.Handler
{
    public class UserCommandsHandler : ResponseHandler, IRequestHandler<AddUserCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly IHttpContextAccessor httpContext;
        private readonly IEmailService emailService;

        public UserCommandsHandler(IMapper mapper, UserManager<AppUser> userManager, IHttpContextAccessor httpContext, IEmailService emailService)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.httpContext = httpContext;
            this.emailService = emailService;
        }
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user != null) return BadRequest<string>("Email is already in use.");
            user = await userManager.FindByNameAsync(request.UserName);
            if (user != null) return BadRequest<string>("Username is already in use.");


            var newUser = mapper.Map<AppUser>(request);

            var Result = await userManager.CreateAsync(newUser, request.Password);

            if (Result.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)); //important to avoid "/ or +" in generated token
                var requestContext = httpContext.HttpContext.Request;
                var confirmationLink = $"{requestContext.Scheme}://{requestContext.Host}/api/authentication/confirm-email?Id={newUser.Id}&token={encodedToken}";
                var subject = "Confirm Your Email";
                var htmlContent = $"<h1>Welcome!</h1><p>Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.</p>";
                await emailService.SendEmailAsync(newUser.Email, subject, htmlContent);
                return Success<string>("Registration successful. Please check your email to confirm your account.");
            }
            var error = Result.Errors.Select(x => x.Description).FirstOrDefault() ?? "";
            return BadRequest<string>(error);






        }
    }
}
