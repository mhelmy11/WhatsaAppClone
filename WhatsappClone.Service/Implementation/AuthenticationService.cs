using Microsoft.EntityFrameworkCore;
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
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Infrastructure.Repositories;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly JwtSettings jwtSettings;

        public IRefreshToken RefreshTokenRepo { get; }
        #endregion
        #region Constructors
        public AuthenticationService(JwtSettings jwtSettings, IRefreshToken refreshTokenRepo)
        {

            this.jwtSettings = jwtSettings;
            RefreshTokenRepo = refreshTokenRepo;
        }
        #endregion        {
        #region Implementations
        public async Task<JWTResult> GetTokenAfterLogging(AppUser user)
        {

            var GeneratedRefreshToken = GenerateRefreshToken(user);
            var refreshToken = new TokenRefreshing
            {
                CreationDate = GeneratedRefreshToken.Expiration,
                Token = GeneratedRefreshToken.Token,
                ExpiresAt = GeneratedRefreshToken.Expiration,
                IsRevoked = false,
                User = user,
                UserId = user.Id
            };


            //Saving to database....
            await RefreshTokenRepo.AddAsync(refreshToken);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var hashing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var exp = DateTime.UtcNow.AddMinutes(5);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!),
                new Claim("TID",refreshToken.Id.ToString())


            };

            var jwt = new JwtSecurityToken(

                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: exp,
                signingCredentials: hashing
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var accessToken = new AccessToken { Token = token, Expiration = exp };

            return new JWTResult() { AccessToken = accessToken, RefreshToken = GeneratedRefreshToken };
        }
        public RefreshToken GenerateRefreshToken(AppUser user)
        {
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            return new RefreshToken
            {
                Token = refreshToken,
                Expiration = DateTime.UtcNow.AddDays(7),
                PhoneNumber = user.PhoneNumber
            };
        }
        public AccessToken GenerateAccessToken(AppUser user)
        {

            var generatedRefreshToken = GenerateRefreshToken(user);
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var hashing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var exp = DateTime.UtcNow.AddMinutes(1);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!),
                new Claim("TID",generatedRefreshToken.Id.ToString())


            };

            var jwt = new JwtSecurityToken(

                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: exp,
                signingCredentials: hashing
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AccessToken { Token = token, Expiration = exp };
        }

        public TokenRefreshing GetRefreshToken(string token)
        {
            return RefreshTokenRepo.GetRefreshToken(token);
        }


        public void RevokeRefreshToken(string token)
        {
            RefreshTokenRepo.RevokeRefreshToken(token);
        }


        #endregion

    }
}
