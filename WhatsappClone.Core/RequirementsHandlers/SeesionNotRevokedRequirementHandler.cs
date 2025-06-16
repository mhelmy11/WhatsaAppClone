using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.RequirementsHandlers
{
    public class SeesionNotRevokedRequirementHandler
    {
        private readonly IRequirementsService requirementsService;

        public SeesionNotRevokedRequirementHandler(IRequirementsService requirementsService)
        {
            this.requirementsService = requirementsService;
        }

        public async Task<bool> HandleAsync(int tokenId)
        {
            if (tokenId <= 0)
            {
                throw new ArgumentException("Token ID must be greater than zero.", nameof(tokenId));
            }
            var result = await requirementsService.IsSessionRevoked(tokenId);

            return !result; // Return true if the session is not revoked, false otherwise


        }
    }
}
