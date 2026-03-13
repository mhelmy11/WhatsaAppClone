using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Wrappers;

namespace WhatsappClone.Core.Features.Messages.Queries.GetChatMessagesQuery
{
    public class GetChatMessagesQueryHandler : ResponseHandler, IRequestHandler<GetChatMessagesQuery, Response<CursorPagedResult<GetChatMessagesResult>>>
    {
        public async Task<Response<CursorPagedResult<GetChatMessagesResult>>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
