using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Users.Commands.Models
{
    public class ForgetPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
    }
}
