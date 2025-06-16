using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories
{
    public class RefreshTokenRepo : Repo<TokenRefreshing>, IRefreshToken
    {
        private readonly Context dbContext;

        public RefreshTokenRepo(Context dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public TokenRefreshing GetRefreshToken(string token)
        {
            return dbContext.RefreshTokens.Include(r => r.User).FirstOrDefault(x => x.RefreshToken == token);

        }

        public void RevokeRefreshToken(string token)
        {
            var refreshToken = dbContext.RefreshTokens.FirstOrDefault(x => x.RefreshToken == token);
            refreshToken.IsRevoked = true;
            dbContext.RefreshTokens.Update(refreshToken);
        }
    }
}

