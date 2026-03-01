using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Contacts.Commands
{
    public class AddContactCommand : IRequest<Response<string>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName => FirstName +" "+ LastName;

        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
