using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Users.Commands.BlockUser
{
    public class BlockUserCommand : IRequest<Response<string>>
    {
        public string BlockedUserId { get; set; }
    }
}
