using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings jwtSettings;
        private ConcurrentDictionary<string, RefreshToken> refreshTokensDic;

        public AuthenticationService(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
            refreshTokensDic = new ConcurrentDictionary<string, RefreshToken>();
        }
        public async Task<JWTResult> GetToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var hashing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var exp = DateTime.Now.AddMinutes(15);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!)


            };

            var jwt = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: exp,
                signingCredentials: hashing
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var refreshToken = GenerateRefreshToken();
            refreshToken.PhoneNumber = user.PhoneNumber;

            refreshTokensDic.AddOrUpdate(user.PhoneNumber, refreshToken, (key, oldValue) => refreshToken);


            return new JWTResult() { AccessToken = token, RefreshToken = refreshToken };
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return new RefreshToken
            {
                Token = refreshToken,
                Expiration = DateTime.Now.AddDays(7) // Set expiration for 7 days
            };


        }
    }
}
