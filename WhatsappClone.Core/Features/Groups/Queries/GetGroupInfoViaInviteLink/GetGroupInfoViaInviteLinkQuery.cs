using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Queries.GetGroupInfoViaInviteLink
{
    public record GetGroupInfoViaInviteLinkQuery(string InviteCode) : IRequest<Response<GetGroupInfoViaInviteLinkResult>>;
    
}
