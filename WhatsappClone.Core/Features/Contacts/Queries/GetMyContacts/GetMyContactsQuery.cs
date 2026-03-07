using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Wrappers;

namespace WhatsappClone.Core.Features.Contacts.Queries.GetMyContacts
{
    public record GetMyContactsQuery(string? Cursor , int Limit , string? SearchTerm) : IRequest<Response<CursorPagedResult<GetMyContactsResult>>>
    {
    }

   

}
