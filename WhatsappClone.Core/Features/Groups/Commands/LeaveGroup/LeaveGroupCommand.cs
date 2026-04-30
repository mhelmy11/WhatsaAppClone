using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.LeaveGroup
{
    public record LeaveGroupCommand(long GroupId) : IRequest<Response<LeaveGroupResult>>;
   
}
