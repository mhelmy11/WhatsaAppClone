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
                                                    , IRequestHandler<GetChatListQuery, Response<List<ChatDTO>>>
                                                    , IRequestHandler<GetChatByIdQuery, Response<GetChatByIdResponse>>
                                                    , IRequestHandler<GetPaginatedChatsQuery, PaginatedResult<GetPaginatedChatsResponse>>

    {
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        
        public ChatsQueryHandler(IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }
        #region GetChatsListQuery
        public async Task<Response<List<GetChatsResponse>>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
        {
            return Success(new List<GetChatsResponse>());


        }
        #endregion



        #region GetChatByIdQuery
        public async Task<Response<GetChatByIdResponse>> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
        {

           
            return Success(new GetChatByIdResponse());

        }

        public async Task<PaginatedResult<GetPaginatedChatsResponse>> Handle(GetPaginatedChatsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();


        }

        public async Task<Response<List<ChatDTO>>> Handle(GetChatListQuery request, CancellationToken cancellationToken)
        {
throw new NotImplementedException();    
            #endregion
        }
} }