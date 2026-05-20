using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Messages.Events;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Groups.Commands.UpdateGroupInfo
{
    public class UpdateGroupInfoCommandHandler : ResponseHandler, IRequestHandler<UpdateGroupInfoCommand, Response<UpdateGroupInfoResult>>
    {
        private readonly CurrentUserService currentUserService;
        private readonly SqlDBContext dBContext;
        private readonly IMongoDBFactory mongoDBFactory;

        public UpdateGroupInfoCommandHandler(
            CurrentUserService currentUserService,
            SqlDBContext dBContext,
            IMediator mediator,
            IMongoDBFactory mongoDBFactory)
        {
            this.currentUserService = currentUserService;
            this.dBContext = dBContext;
            this.mongoDBFactory = mongoDBFactory;
        }
        public async Task<Response<UpdateGroupInfoResult>> Handle(UpdateGroupInfoCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var targetGroup = await dBContext.Groups
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

            if (targetGroup == null)
            {
                return BadRequest<UpdateGroupInfoResult>("this group is not found");
            }

            var member = await dBContext.GroupMembers
                .FirstOrDefaultAsync(gm => gm.UserId == currentUserId && gm.GroupId == request.GroupId, cancellationToken);

            if (member == null || member.Status != MemberStatus.Active)
            {
                return BadRequest<UpdateGroupInfoResult>("You are not a member of this group");
            }

            var isAdmin = member.Role == MemberRole.Admin;
            if (!targetGroup.EditGroupSettings && !isAdmin)
            {
                return BadRequest<UpdateGroupInfoResult>("You Can't Edit Group's Info");
            }

            var hasChanges = false;
            var systemEvents = new System.Collections.Generic.List<SystemEvent>();

            if (!string.IsNullOrWhiteSpace(request.GroupDescription) &&
                !string.Equals(targetGroup.Description, request.GroupDescription, StringComparison.Ordinal))
            {
                var oldDescription = targetGroup.Description;
                targetGroup.Description = request.GroupDescription;
            }

            if (!string.IsNullOrWhiteSpace(request.GroupPic) &&
                !string.Equals(targetGroup.ProfilePicUrl, request.GroupPic, StringComparison.Ordinal))
            {
                var oldPic = targetGroup.ProfilePicUrl;
                targetGroup.ProfilePicUrl = request.GroupPic;
                hasChanges = true;
                systemEvents.Add(new SystemEvent
                {
                    EventType = SystemEventType.EditGroupPic,
                    ActorId = currentUserId,
                    TargetGroupId = request.GroupId,
                    OldValue = oldPic,
                    NewValue = request.GroupPic
                });
            }

            if (!string.IsNullOrWhiteSpace(request.GroupName) &&
                !string.Equals(targetGroup.Name, request.GroupName, StringComparison.Ordinal))
            {
                var oldName = targetGroup.Name;
                targetGroup.Name = request.GroupName;
                hasChanges = true;
                systemEvents.Add(new SystemEvent
                {
                    EventType = SystemEventType.EditGroupName,
                    ActorId = currentUserId,
                    TargetGroupId = request.GroupId,
                    OldValue = oldName,
                    NewValue = request.GroupName
                });
            }

            if (!hasChanges)
            {
                return BadRequest<UpdateGroupInfoResult>("No group info changes were provided");
            }

            await dBContext.SaveChangesAsync(cancellationToken);

            if (systemEvents.Count > 0)
            {
                var messagesCollection = mongoDBFactory.GetCollection<Message>();
                var participants = await dBContext.GroupMembers
                    .Where(gm => gm.GroupId == request.GroupId && gm.Status == MemberStatus.Active)
                    .Select(gm => gm.UserId)
                    .ToListAsync(cancellationToken);

                foreach (var systemEvent in systemEvents)
                {
                    var systemMessage = new Message
                    {
                        ChatId = request.GroupId,
                        SenderId = currentUserId,
                        RecipientType = ConversationType.Group,
                        RecipientId = null,
                        MessageType = MessageType.System,
                        Timestamp = DateTime.UtcNow,
                        Content = new MessageContent
                        {
                            SystemEvent = systemEvent
                        }
                    };

                    await messagesCollection.InsertOneAsync(systemMessage, cancellationToken: cancellationToken);

                   // fire event
                   //to notify group members about the update
               
                }
            }

            return Success<UpdateGroupInfoResult>(new UpdateGroupInfoResult(GroupId: request.GroupId));
        }
    }
}
