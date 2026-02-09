using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces
{
    public interface IRefreshTokenAuditRepository : ISqlBaseRepository<RefreshTokenAudit>
    {
        public RefreshTokenAudit GetRefreshToken(string token);

        public void RevokeRefreshToken(string token);
    }
}
