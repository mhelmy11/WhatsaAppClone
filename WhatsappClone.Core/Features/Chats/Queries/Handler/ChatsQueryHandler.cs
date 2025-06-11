using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Chats.Queries.Models;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;
using MediatR;
using WhatsappClone.Core.Features.Chats.Queries.Results;

namespace WhatsappClone.Core.Features.Chats.Queries.Handler
{
    public class ChatsQueryHandler : ResponseHandler, IRequestHandler<GetChatsQuery, Response<List<GetChatsResponse>>>
                                                    , IRequestHandler<GetChatByIdQuery, Response<GetChatByIdResponse>>
    {
        private readonly IChatService chatService;
        private readonly IMapper mapper;

        public ChatsQueryHandler(IChatService chatService, IMapper mapper)
        {
            this.chatService = chatService;
            this.mapper = mapper;
        }
        #region GetChatsListQuery
        public async Task<Response<List<GetChatsResponse>>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
        {
            var Chats = await chatService.GetChatsAsync();
            var MappedResult = mapper.Map<List<GetChatsResponse>>(Chats);
            return Success(MappedResult);


        }
        #endregion



        #region GetChatByIdQuery
        public async Task<Response<GetChatByIdResponse>> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
        {

            var Chat = await chatService.GetChatByIdAsync(request.Id);
            if (Chat == null)
            {
                return NotFound<GetChatByIdResponse>("Chat not found");
            }
            var MappedResult = mapper.Map<GetChatByIdResponse>(Chat);
            return Success(MappedResult);

        }
        #endregion
    }
}
