using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Groups.Commands.Models;
using WhatsappClone.Data.Enums;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Groups.Commands.Handler
{
    public class GroupCommandsHandler : ResponseHandler, IRequestHandler<CreateGroupCommand, Response<Guid>>
                                                       , IRequestHandler<RemoveMemberCommand, Response<string>>
                                                       , IRequestHandler<AddListOfMembersCommand, Response<List<string>>>

    {
        private readonly IMapper mapper;
        private readonly IMessagesService messagesService;
        private readonly IGroupService groupService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IFileService fileService;
        private readonly UserManager<AppUser> userManager;

        public GroupCommandsHandler(IMapper mapper,
                                    IMessagesService messagesService,
                                    IGroupService groupService,
                                    IHttpContextAccessor httpContextAccessor,
                                    IFileService fileService,
                                    UserManager<AppUser> userManager
            )
        {
            this.mapper = mapper;
            this.messagesService = messagesService;
            this.groupService = groupService;
            this.httpContextAccessor = httpContextAccessor;
            this.fileService = fileService;
            this.userManager = userManager;
        }
        public async Task<Response<Guid>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {

            var creatorId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var creator = await userManager.FindByIdAsync(creatorId);
            var group = mapper.Map<Group>(request);
            if (request.GroupPictureUrl != null)
            {


                var picUrl = await fileService.SaveFileAsync(request.GroupPictureUrl, "GroupPics");
                group.GroupPictureUrl = picUrl;
            }

            group.CreatorId = creatorId;

            //create group
            var createdGroup = await groupService.CreateGroup(group, creatorId, request.UserIDs);


            return Success(createdGroup.Id, "Group Created Successfully");

        }


        public async Task<Response<string>> Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
        {
            var adminId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //check if adder is admin of the group
            var isAdmin = await groupService.IsUserAdmin(adminId, request.GroupId);
            if (!isAdmin)
            {
                return BadRequest<string>("You are not authorized to remove members to this group");
            }
            var member = await userManager.FindByIdAsync(request.UserId);
            var admin = await userManager.FindByIdAsync(adminId);




            await groupService.RemoveMemberFromGroup(userId: request.UserId, groupId: request.GroupId);
            if (member == null)
            {
                return BadRequest<string>("Member not found");
            }

            await messagesService.AddMessage(adminId, "", request.GroupId, $"{member.Id} has been removed from the group");

            return Success($"{admin.FullName} removed {member.FullName}");

        }

        public async Task<Response<List<string>>> Handle(AddListOfMembersCommand request, CancellationToken cancellationToken)
        {
            var actorId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //check if adder is admin of the group
            var isAdmin = await groupService.IsUserAdmin(actorId, request.groupId);
            if (!isAdmin)
            {
                return BadRequest<List<string>>("You are not authorized to remove members to this group");
            }

            //1. create a service to handle adding members to a group
            var result = await groupService.AddListOfMembers(actorId, request.groupId, request.members);
            //1.1 add members to group
            //1.2 add system message content for each member added after serializing it to json
            //2. return the list of members added
            return Success(result, "Members added successfully");
        }
    }
}
