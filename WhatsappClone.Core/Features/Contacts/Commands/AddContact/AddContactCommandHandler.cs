using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Contacts.Commands.AddContact
{
    public class AddContactCommandHandler : ResponseHandler,
        IRequestHandler<AddContactCommand, Response<string>>
    {
        public async Task<Response<string>> Handle(AddContactCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
