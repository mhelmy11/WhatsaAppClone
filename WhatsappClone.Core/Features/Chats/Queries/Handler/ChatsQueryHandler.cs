using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Chats.Queries.Models;
using WhatsappClone.Core.Features.Chats.Queries.Results;
using WhatsappClone.Core.Wrapper;
using WhatsappClone.Core.Wrappers;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Chats.Queries.Handler
{
    public class ChatsQueryHandler : ResponseHandler, IRequestHandler<GetChatsQuery, Response<List<GetChatsResponse>>>
                                                    , IRequestHandler<GetChatByIdQuery, Response<GetChatByIdResponse>>
                                                    , IRequestHandler<GetPaginatedChatsQuery, PaginatedResult<GetPaginatedChatsResponse>>
                                                    , IRequestHandler<GetChatListQuery, Response<List<ChatDTO>>>

    {
        private readonly IChatService chatService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ChatsQueryHandler(IChatService chatService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.chatService = chatService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
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

        public async Task<PaginatedResult<GetPaginatedChatsResponse>> Handle(GetPaginatedChatsQuery request, CancellationToken cancellationToken)
        {


            var FilterQuery = chatService.FilterChatPaginatedQueryable(request.orderBy, request.search);
            var PaginatedList = await mapper.ProjectTo<GetPaginatedChatsResponse>(FilterQuery)
                                            .ToPaginatedListAsync(request.pageNumber, request.pageSize);

            return PaginatedList;

        }

        public async Task<Response<List<ChatDTO>>> Handle(GetChatListQuery request, CancellationToken cancellationToken)
        {

            var id = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var msgs = chatService.GetChatListOfCurrentUSer(id);

            return Success(msgs);
            #endregion
        }
    }
}
