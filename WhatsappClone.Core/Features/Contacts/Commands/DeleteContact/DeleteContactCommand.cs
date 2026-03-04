using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Contacts.Commands
{
    public class DeleteContactCommand : IRequest<Response<string>>
    {
        public string ContactId { get; set; }
    }
}
