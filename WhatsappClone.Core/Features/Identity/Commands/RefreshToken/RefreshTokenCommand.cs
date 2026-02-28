using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Identity.Commands
{
    public class RefreshTokenCommand : IRequest<Response<RefreshTokenResult>>
    {

        public long? UserId { get; set; }

        public string RefreshToken { get; set; } 
    }
}
