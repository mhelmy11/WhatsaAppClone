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

        public GroupQueriesHandler(IMessagesService messagesService, IGroupService groupService, IHttpContextAccessor httpContextAccessor)
        {
            this.messagesService = messagesService;
            this.groupService = groupService;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<List<ChatDTO>>> Handle(GetGroupListQuery request, CancellationToken cancellationToken)
        {

            var currentuserId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var groupsIDs = groupService.GetGroupIdsOfUser(currentuserId);
            var lastMessages = messagesService.GetLasMessageOfGroupsIDs(groupsIDs, currentuserId);

            return Success(lastMessages, "Group List retrieved successfully");

        }

        public Task<Response<GetGroupInviteInfoResult>> Handle(GetGroupInviteInfoQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
