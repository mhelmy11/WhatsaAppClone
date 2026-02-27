using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Infrastructure.Repositories;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation;

public class AuthenticationService : IAuthenticationService
{
    #region Fields
    private readonly JwtSettings jwtSettings;
    private readonly SqlDBContext sqlDBContext;

    public IRefreshTokenAuditRepository RefreshTokenRepo { get; }
    #endregion
    #region Constructors
    public AuthenticationService(IOptions<JwtSettings> jwtSettings, IRefreshTokenAuditRepository refreshTokenRepo , SqlDBContext sqlDBContext )
    {

        this.jwtSettings = jwtSettings.Value;
        RefreshTokenRepo = refreshTokenRepo;
        this.sqlDBContext = sqlDBContext;
    }
    #endregion        {
    #region Implementations
    public async Task<JWTResult> GetTokenAfterLogin(User user)
    {

        var GeneratedRefreshToken = GenerateRefreshToken(user);
        var refreshToken = new RefreshTokenAudit
        {
            CreatedAt = GeneratedRefreshToken.Expiration,
            Token = GeneratedRefreshToken.Token,
            ExpiresAt = GeneratedRefreshToken.Expiration,
            IsRevoked = false,
            User = user,
            UserId = user.Id
        };

        //Saving to database....
        await sqlDBContext.AddAsync(refreshToken);

        var accessToken = GenerateAccessToken(user, refreshToken.Id.ToString());

        return new JWTResult() { AccessToken = accessToken, RefreshToken = GeneratedRefreshToken };
    }
    public RefreshToken GenerateRefreshToken(User user)
    {
        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        return new RefreshToken
        {
            Token = refreshToken,
            Expiration = DateTime.UtcNow.AddDays(7),
            PhoneNumber = user.PhoneNumber,
            
        };
    }
    public AccessToken GenerateAccessToken(User user , string? TID)
    {

        var generatedRefreshToken = GenerateRefreshToken(user);
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var hashing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var exp = DateTime.UtcNow.AddMinutes(10);
        var claims = new List<Claim> { 
            
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!),
                


            };
        if (TID != null)
        {
            claims.Add(new Claim("TID", TID));
        }

        var jwt = new JwtSecurityToken(

            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: exp,
            signingCredentials: hashing
            );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new AccessToken { Token = token, Expiration = exp , UserId = user.Id.ToString() };
    }
    public RefreshTokenAudit GetRefreshToken(string token)
    {
        return sqlDBContext.RefreshTokenAudits.FirstOrDefault(r=>r.Token == token)!;
    }
    public void RevokeRefreshToken(string token)
    {
        var refreshToken = sqlDBContext.RefreshTokenAudits.FirstOrDefault(x => x.Token == token);
        refreshToken.IsRevoked = true;
        sqlDBContext.RefreshTokenAudits.Update(refreshToken);
    }


    #endregion

}
