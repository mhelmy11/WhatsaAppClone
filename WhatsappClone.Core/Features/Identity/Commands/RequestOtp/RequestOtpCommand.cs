using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Identity.Commands
{
    public class RequestOtpCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }

        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
