using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IAuthenticationService authenticationService;

        public AuthenticationHandler(UserManager<AppUser> userManager, IAuthenticationService authenticationService)
        {
            this.userManager = userManager;
            this.authenticationService = authenticationService;
        }

        public async Task<Response<JWTResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = userManager.FindByPhoneNumber(request.PhoneNumber);
            if (user == null) return BadRequest<JWTResult>("User not found. Please register first.");
            var isValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isValid) return BadRequest<JWTResult>("Invalid password. Please try again.");
            var jwtResult = await authenticationService.GetTokenAfterLogging(user);
            if (jwtResult == null) return BadRequest<JWTResult>("Token generation failed. Please try again.");

            return Success(jwtResult, "Login successful. Token generated successfully.");


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
    }
}
