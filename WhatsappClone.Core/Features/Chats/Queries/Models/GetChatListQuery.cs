using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Chats.Queries.Results;

namespace WhatsappClone.Core.Features.Chats.Queries.Models
{
    public class GetChatListQuery : IRequest<Response<List<GetChatListResult>>>
    {
    }
}
