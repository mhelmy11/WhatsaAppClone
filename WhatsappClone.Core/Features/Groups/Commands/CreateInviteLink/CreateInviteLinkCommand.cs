using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.CreateInviteLink
{
    public class CreateInviteLinkCommand : IRequest<Response<CreateInviteLinkResult>>
    {
        public long GroupId { get; set; }

    }
}
