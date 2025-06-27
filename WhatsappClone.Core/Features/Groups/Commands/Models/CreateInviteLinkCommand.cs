using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.Models
{
    public class CreateInviteLinkCommand : IRequest<Response<string>>
    {
        public Guid groupId { get; set; }
    }
}
