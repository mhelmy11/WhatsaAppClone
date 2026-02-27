using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Service.Abstract
{
    public interface IAuthenticationService
    {

        public Task<JWTResult> GetTokenAfterLogin(User user);

        public AccessToken GenerateAccessToken(User user , string? TID);

        public RefreshToken GenerateRefreshToken(User user);

        public RefreshTokenAudit GetRefreshToken(string token);

        public void RevokeRefreshToken(string token);




    }
}
