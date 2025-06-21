using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Contacts.Queries.Results;

namespace WhatsappClone.Core.Features.Contacts.Queries.Models
{
    public class GetContactsQuery : IRequest<Response<List<GetContactsQueryResult>>>
    {
    }
}
