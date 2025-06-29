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
        private readonly IMessagesService messagesService;
        private readonly IGroupService groupService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public GroupQueriesHandler(IMessagesService messagesService, IGroupService groupService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.messagesService = messagesService;
            this.groupService = groupService;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }
        public async Task<Response<List<ChatDTO>>> Handle(GetGroupListQuery request, CancellationToken cancellationToken)
        {

            var currentuserId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var groupsIDs = groupService.GetGroupIdsOfUser(currentuserId);
            var lastMessages = messagesService.GetLasMessageOfGroupsIDs(groupsIDs, currentuserId);

            return Success(lastMessages, "Group List retrieved successfully");

        }

        public async Task<Response<GetGroupInviteInfoResult>> Handle(GetGroupInviteInfoQuery request, CancellationToken cancellationToken)
        {
            var groupFromDB = groupService.GetGroupIdByInviteCode(request.inviteCode);

            //mapping
            var groupInfo = mapper.Map<GetGroupInviteInfoResult>(groupFromDB);

            return Success(groupInfo);


        }
    }
}
