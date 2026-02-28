using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Identity.Commands
{
    public class RefreshTokenCommandHandler : ResponseHandler, IRequestHandler<RefreshTokenCommand, Response<RefreshTokenResult>>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SqlDBContext dBContext;
        private readonly UserManager<User> userManager;
        private readonly IAuthenticationService authenticationService;

        public RefreshTokenCommandHandler(
            IHttpContextAccessor httpContextAccessor ,
            SqlDBContext dBContext ,
            UserManager<User> userManager ,
            IAuthenticationService authenticationService
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dBContext = dBContext;
            this.userManager = userManager;
            this.authenticationService = authenticationService;
        }
        public async Task<Response<RefreshTokenResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            //get refresh token from db
            var refreshTokenFromDb = await dBContext.RefreshTokenAudits.Where(rt=>rt.Token == request.RefreshToken).FirstOrDefaultAsync();
           
            if (refreshTokenFromDb == null || refreshTokenFromDb.ExpiresAt > DateTime.UtcNow ) 
            {
                return BadRequest<RefreshTokenResult>("Invalid Refresh Token");
            }
            if (!refreshTokenFromDb.IsRevoked)
            {
                dBContext.Remove(refreshTokenFromDb);
                await dBContext.SaveChangesAsync();
                return BadRequest<RefreshTokenResult>("Security breach detected. Please login again.");

            }
            //revoke it
            refreshTokenFromDb.RevokedAt = DateTime.UtcNow;
            refreshTokenFromDb.IsRevoked = true;
            await dBContext.SaveChangesAsync();


            //generate new refresh + access tokens

            var tokens = await authenticationService.GetTokenAfterLogin(refreshTokenFromDb.User);

            return Success(new RefreshTokenResult { tokens = tokens });







            
        }
    }
}
