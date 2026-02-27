using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Groups.Queries.Models;
using WhatsappClone.Core.Features.Groups.Queries.Results;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Groups.Queries.Handler
{
    public class GroupQueriesHandler : ResponseHandler
                                        , IRequestHandler<GetGroupListQuery, Response<List<ChatDTO>>>
                                        , IRequestHandler<GetGroupInviteInfoQuery, Response<GetGroupInviteInfoResult>>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public GroupQueriesHandler(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {

            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }
        public async Task<Response<List<ChatDTO>>> Handle(GetGroupListQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();


        }

        public async Task<Response<GetGroupInviteInfoResult>> Handle(GetGroupInviteInfoQuery request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();



        }
    }
}
