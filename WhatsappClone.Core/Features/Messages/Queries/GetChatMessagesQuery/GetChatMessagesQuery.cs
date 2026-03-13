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
    public record GetChatMessagesQuery(long ChatId , string? Cursor, int Limit) : IRequest<Response<CursorPagedResult<GetChatMessagesResult>>>
    {

    }
}
