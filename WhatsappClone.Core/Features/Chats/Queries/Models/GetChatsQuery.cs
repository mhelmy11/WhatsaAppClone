using MoMediatoR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Chats.Results;
using WhatsappClone.Core.Responses;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Core.Features.Chats.Queries.Models
{
    public class GetChatsQuery : IRequest<Response<List<GetChatsResponse>>>
    {

    }

}
