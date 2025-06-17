using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Authentication.Commands.Models
{
    public class EmailConfirmCommand : IRequest<Response<string>>
    {
        public string Id { get; set; }
        public string Token { get; set; }


    }
}
