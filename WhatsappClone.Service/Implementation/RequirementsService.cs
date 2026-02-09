using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class RequirementsService : IRequirementsService
    {
        private readonly IRefreshTokenAuditRepository refreshTokenRepo;

        public RequirementsService(IRefreshTokenAuditRepository refreshTokenRepo)
        {
            this.refreshTokenRepo = refreshTokenRepo;
        }


        public async Task<bool> IsSessionRevoked(int tid)
        {
            var refreshToken = await refreshTokenRepo.GetByIdAsync(tid);
            if (refreshToken == null)
            {
                return false;
            }

            return refreshToken.IsRevoked;
        }
    }
}
