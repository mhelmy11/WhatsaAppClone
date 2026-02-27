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
    public class RefreshTokenAuditRepository : SqlBaseRepository<RefreshTokenAudit>, IRefreshTokenAuditRepository
    {
        private readonly SqlDBContext dbContext;

        public RefreshTokenAuditRepository(SqlDBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public RefreshTokenAudit GetRefreshToken(string token)
        {
            return dbContext.RefreshTokenAudits.Include(r => r.User).FirstOrDefault(x => x.Token == token);

        }

        public void RevokeRefreshToken(string token)
        {
            var refreshToken = dbContext.RefreshTokenAudits.FirstOrDefault(x => x.Token == token);
            refreshToken.IsRevoked = true;
            dbContext.RefreshTokenAudits.Update(refreshToken);
        }
    }
}

