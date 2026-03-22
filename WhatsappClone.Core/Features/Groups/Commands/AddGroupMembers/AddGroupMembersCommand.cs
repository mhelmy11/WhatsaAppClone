using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.AddGroupMembers
{
    public record AddGroupMembersCommand(
        long GroupId ,
        List<long>Members
        ) : IRequest<Response<AddGroupMembersResult>>;
  
}
