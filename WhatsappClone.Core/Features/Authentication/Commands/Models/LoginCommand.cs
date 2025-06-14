using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Core.Features.Authentication.Commands.Models
{
    public class LoginCommand : IRequest<Response<JWTResult>>
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

    }
}
