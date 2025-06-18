using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Authentication.Commands.Models;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Authentication.Commands.Handler
{
    public class AuthenticationHandler : ResponseHandler, IRequestHandler<LoginCommand, Response<JWTResult>>
                                                        , IRequestHandler<RefreshTokenCommand, Response<JWTResult>>
                                                        , IRequestHandler<EmailConfirmCommand, Response<string>>


    {
        private readonly ILogger<AuthenticationHandler> logger;
        private readonly UserManager<AppUser> userManager;
        private readonly IAuthenticationService authenticationService;
        private readonly SignInManager<AppUser> signInManager;

        public AuthenticationHandler(ILogger<AuthenticationHandler> logger, UserManager<AppUser> userManager, IAuthenticationService authenticationService, SignInManager<AppUser> signInManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.authenticationService = authenticationService;
            this.signInManager = signInManager;
        }

        public async Task<Response<JWTResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                logger.LogWarning($"A request was made with a user not found: {request.Email}");

                return BadRequest<JWTResult>("User not found. Please register first.");
            }
            var passwordCheck = await userManager.CheckPasswordAsync(user, request.Password);
            var result = await signInManager.PasswordSignInAsync(user, request.Password, false, true);

            if (result.Succeeded)
            {
                var jwtResult = await authenticationService.GetTokenAfterLogging(user);
                if (jwtResult == null) return BadRequest<JWTResult>("Token generation failed. Please try again.");

                return Success(jwtResult, "Login successful. Token generated successfully.");
            }



            if (!result.Succeeded)
            {
                if (passwordCheck == false) return BadRequest<JWTResult>("Invalid credentials. Please try again.");

                if (passwordCheck && result.IsNotAllowed) return BadRequest<JWTResult>("User is not allowed to login. Please confirm your email first.");
                if (result.IsLockedOut) return BadRequest<JWTResult>("Account is locked out. Please try again later after 1 min.");
            }

            return BadRequest<JWTResult>("Invalid credentials. Please try again.");


        }

        public async Task<Response<JWTResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            //Return new Access Token and the same refresh token if it is not expired or revoked.

            //get the refresh token for user 
            var refreshToken = authenticationService.GetRefreshToken(request.RefreshToken);
            if (refreshToken == null)
            {
                return BadRequest<JWTResult>("Invalid refresh token. Redirect to Login Page");
            }

            if (refreshToken.ExpiryDate < DateTime.Now)
            {

                authenticationService.RevokeRefreshToken(request.RefreshToken);
                return BadRequest<JWTResult>("Refresh token is Expired. Please login again to get a new token.");

            }
            if (refreshToken.IsRevoked)
            {

                //Hacker attempted to use a revoked token.
                //TODO....
                return BadRequest<JWTResult>("Refresh token is revoked. Please login again to get a new token.");
            }


            //else Generate new tokens(Refresh and access) and return them and update the refresh token in DB.
            //Revoke the old refresh token
            authenticationService.RevokeRefreshToken(request.RefreshToken);
            var jwtResult = await authenticationService.GetTokenAfterLogging(refreshToken.User);
            return Success(jwtResult);
        }

        public async Task<Response<string>> Handle(EmailConfirmCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return BadRequest<string>("User not found.");
            }

            // Decoding the token
            try
            {
                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

                var result = await userManager.ConfirmEmailAsync(user, decodedToken);

                if (result.Succeeded)
                {
                    return Success("Email confirmed successfully!");
                }

                return BadRequest<string>("Email confirmation failed.");
            }
            catch (FormatException)
            {
                return BadRequest<string>("Invalid token format.");
            }
        }
    }
}
