using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Groups.Commands.UpdateGroupInfo;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Groups.Commands.UpdateGroupSettings
{
    public class UpdateGroupSettingsCommandHandler : ResponseHandler, IRequestHandler<UpdateGroupSettingsCommand, Response<UpdateGroupSettingsResult>>
    {
        private readonly CurrentUserService currentUserService;
        private readonly MongoDBFactory mongoDBFactory;
        private readonly SqlDBContext dBContext;

        public UpdateGroupSettingsCommandHandler(CurrentUserService currentUserService , MongoDBFactory mongoDBFactory , SqlDBContext dBContext )
        {
            this.currentUserService = currentUserService;
            this.mongoDBFactory = mongoDBFactory;
            this.dBContext = dBContext;
        }
        public async Task<Response<UpdateGroupSettingsResult>> Handle(UpdateGroupSettingsCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var targetGroup = await dBContext.Groups
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

            if (targetGroup == null)
            {
                return BadRequest<UpdateGroupSettingsResult>("this group is not found");
            }

            var member = await dBContext.GroupMembers
                .FirstOrDefaultAsync(gm => gm.UserId == currentUserId && gm.GroupId == request.GroupId, cancellationToken);

            if (member == null || member.Status != MemberStatus.Active)
            {
                return BadRequest<UpdateGroupSettingsResult>("You are not a member of this group");
            }

            var isAdmin = member.Role == MemberRole.Admin;
            if (!targetGroup.EditGroupSettings && !isAdmin)
            {
                return BadRequest<UpdateGroupSettingsResult>("You Can't Edit Group's Settings");
            }

            var hasChanges = false;
            var systemEvents = new System.Collections.Generic.List<SystemEvent>();

            if (request.EditGroupSettings.HasValue)
            {
                hasChanges = true;
                systemEvents.Add(new SystemEvent
                {
                    EventType = SystemEventType.EditGroupSettings,
                    ActorId = currentUserId,
                    TargetGroupId = request.GroupId,
                });
            }

            if (request.SendNewMessages.HasValue)
            {
                hasChanges = true;
                systemEvents.Add(new SystemEvent
                {
                    EventType = SystemEventType.SendNewMessages,
                    ActorId = currentUserId,
                    TargetGroupId = request.GroupId,
                    
                });
            }

            if (request.AddOtherMembers.HasValue)
            {
                hasChanges = true;
                systemEvents.Add(new SystemEvent
                {
                    EventType = SystemEventType.AddOtherMembers,
                    ActorId = currentUserId,
                    TargetGroupId = request.GroupId,
                });
            }

            if (!hasChanges)
            {
                return BadRequest<UpdateGroupSettingsResult>("No group info changes were provided");
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

            return Success<UpdateGroupSettingsResult>(new UpdateGroupSettingsResult(GroupId: request.GroupId));
        }
    }
}
