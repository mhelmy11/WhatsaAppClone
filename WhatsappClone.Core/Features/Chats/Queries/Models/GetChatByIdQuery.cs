using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Chats.Results;

namespace WhatsappClone.Core.Features.Chats.Queries.Models;

public class GetChatByIdQuery : IRequest<Response<GetChatByIdResponse>>
{
    public int Id { set; get; }

    public GetChatByIdQuery(int Id)
    {
        this.Id = Id;
    }

}
