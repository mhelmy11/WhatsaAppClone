using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Contacts.Commands.Results;

namespace WhatsappClone.Core.Features.Contacts.Commands.Models
{
    public class AddContactCommand : IRequest<Response<AddContactResult>>
    {
        public string? userId { get; set; }
        public string? contactId { get; set; }
        public string FName { get; set; } = "";
        public string? LName { get; set; } = "";
        public string PhoneNumber { get; set; }

    }
}
