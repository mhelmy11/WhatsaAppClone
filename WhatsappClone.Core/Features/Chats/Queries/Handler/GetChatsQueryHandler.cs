using AutoMapper;
using MoMediatoR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Chats.Queries.Models;
using WhatsappClone.Core.Features.Chats.Results;
using WhatsappClone.Core.Responses;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Chats.Queries.Handler
{
    public class GetChatsQueryHandler : ResponseHandler, IRequestHandler<GetChatsQuery, Response<List<GetChatsResponse>>>
    {
        private readonly IChatService chatService;
        private readonly IMapper mapper;

        public GetChatsQueryHandler(IChatService chatService, IMapper mapper)
        {
            this.chatService = chatService;
            this.mapper = mapper;
        }
        public async Task<Response<List<GetChatsResponse>>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
        {
            var Chats = await chatService.GetChatsAsync();
            var MappedResult = mapper.Map<List<GetChatsResponse>>(Chats);
            return Success(MappedResult);


        }
    }
}
