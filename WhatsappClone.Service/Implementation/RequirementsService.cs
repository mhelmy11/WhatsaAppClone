using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class RequirementsService : IRequirementsService
    {
        private readonly SqlDBContext dBContext;

        public RequirementsService( SqlDBContext dBContext )
        {
            this.dBContext = dBContext;
        }


        public async Task<bool> IsSessionRevoked(int tid)
        {
            var refreshToken = await dBContext.RefreshTokenAudits.FirstOrDefaultAsync(r=>r.Id == tid);
            if (refreshToken == null)
            {
                return false;
            }

            return refreshToken.IsRevoked;
        }
    }
}
