using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Groups.Commands.JoinGroupViaInviteLink
{
    public record JoinGroupViaInviteLinkResult(long JoinedMember , long GroupId);
    
}
