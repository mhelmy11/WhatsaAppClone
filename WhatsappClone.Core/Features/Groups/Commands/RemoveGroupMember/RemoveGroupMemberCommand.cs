using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.RemoveGroupMember
{
    public record RemoveGroupMemberCommand(long GroupId , long TargetUserId) : IRequest<Response<RemoveGroupMemberResult>>;
    
}
