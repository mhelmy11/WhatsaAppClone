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
                                                       , IRequestHandler<LeaveGroupCommand, Response<string>>
                                                       , IRequestHandler<EditGroupDescriptionCommand, Response<string>>
                                                       , IRequestHandler<EditGroupNameCommand, Response<string>>
                                                       , IRequestHandler<EditGroupPhotoCommand, Response<string>>

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

            var isAdmin = await groupService.IsUserAdmin(adminId, request.GroupId);
            if (!isAdmin)
            {
                return BadRequest<string>("You are not authorized to remove members to this group");
            }

            await groupService.RemoveMemberFromGroup(request.UserId, request.GroupId, adminId);


            return Success($"{adminId} removed {request.UserId.Count()} member(s)");

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

            var result = await groupService.AddListOfMembers(actorId, request.groupId, request.members);
            return Success(result, "Members added successfully");
        }

        public async Task<Response<string>> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await groupService.LeaveGroup(userId, request.GroupId);
            return Success("User left the group successfully");
            //Leave Service

        }



        public async Task<Response<string>> Handle(EditGroupPhotoCommand request, CancellationToken cancellationToken)
        {
            var actorId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //check if adder is admin of the group
            var isAdmin = groupService.IsUserAdmin(actorId, request.GroupId);
            if (!isAdmin.Result)
            {
                return BadRequest<string>("You are not authorized to edit group photo");
            }

            var group = groupService.GetGroupById(request.GroupId);
            var oldPicUrl = group.GroupPictureUrl;


            if (request.GroupProfilePic != null)
            {


                var picUrl = await fileService.SaveFileAsync(request.GroupProfilePic, "GroupPics");
                group.GroupPictureUrl = picUrl;
            }

            await groupService.UpdateGroupPic(group, actorId, oldPicUrl);

            return Success("Group photo updated successfully");

        }

        public async Task<Response<string>> Handle(EditGroupNameCommand request, CancellationToken cancellationToken)
        {
            var actorId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //check if adder is admin of the group
            var isAdmin = groupService.IsUserAdmin(actorId, request.GroupId);
            if (!isAdmin.Result)
            {
                return BadRequest<string>("You are not authorized to edit group Name");
            }

            var group = groupService.GetGroupById(request.GroupId);
            var oldName = group.Name;



            await groupService.UpdateGroupName(group, actorId, oldName);

            return Success("Group Name updated successfully");
        }

        public async Task<Response<string>> Handle(EditGroupDescriptionCommand request, CancellationToken cancellationToken)
        {
            var actorId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = groupService.IsUserAdmin(actorId, request.GroupId);
            if (!isAdmin.Result)
            {
                return BadRequest<string>("You are not authorized to edit group description");
            }

            var group = groupService.GetGroupById(request.GroupId);

            await groupService.UpdateGroupDescription(group, actorId);

            return Success("Group Description updated successfully");
        }
    }
}
