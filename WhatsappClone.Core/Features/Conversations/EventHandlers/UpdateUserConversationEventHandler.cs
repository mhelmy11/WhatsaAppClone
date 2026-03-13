using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Messages.Events;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Service.Helpers.WhatsappClone.Shared.Extensions;

namespace WhatsappClone.Core.Features.Conversations.EventHandlers
{
    public class UpdateUserConversationEventHandler : INotificationHandler<MessageCreatedEvent>
    {
        private readonly IMongoDBFactory mongoDBFactory;
        private readonly SqlDBContext dBContext;

        public UpdateUserConversationEventHandler(IMongoDBFactory mongoDBFactory, SqlDBContext dBContext)
        {
            this.mongoDBFactory = mongoDBFactory;
            this.dBContext = dBContext;
        }
        public async Task Handle(MessageCreatedEvent notification, CancellationToken cancellationToken)
        {
            var userConversationCollection = mongoDBFactory.GetCollection<UserConversation>();
            bool isMedia = notification.MessageType != MessageType.Text && notification.MessageType != MessageType.System;

            string targetDisplayName = notification.ConversationType == ConversationType.Group? await dBContext.Groups.Where(g=>g.Id == notification.ChatId).Select(c=>  c.ProfilePicUrl).FirstAsync(cancellationToken) : string.Empty;
            string targetProfilePic = notification.ConversationType == ConversationType.Group ? await dBContext.Groups.Where(g => g.Id == notification.ChatId).Select(c => c.Name).FirstAsync(cancellationToken) : string.Empty;
            var lastMessage = new LastMessage
            {
                MessageId = notification.MessageId,
                SenderId = notification.SenderId,
                Timestamp = notification.SentAt,
                IsDeleted = false,
                IsMedia = isMedia,
                Content = new LastMessageContent
                {
                    Text = notification.TextContent,
                    MediaType = isMedia ? notification.MessageType : null,
                    IsMedia = isMedia,
                    Caption = notification.Caption,
                }
            };
            

            var bulkOperations = new List<WriteModel<UserConversation>>();

            foreach (var participantId in notification.Participants)
            {
                bool isSender = participantId == notification.SenderId;
                bool isMentioned = notification.Mentions?.Contains(participantId) == true;

                long? currentPeerId = notification.ConversationType == ConversationType.Individual
                    ? (isSender ? notification.PeerId : notification.SenderId)
                    : null;

                var filter = Builders<UserConversation>.Filter.And(
                    Builders<UserConversation>.Filter.Eq(uc => uc.ChatId, notification.ChatId),
                    Builders<UserConversation>.Filter.Eq(uc => uc.UserId, participantId)
                );
                if(notification.ConversationType == ConversationType.Individual)
                {
                    if (isSender)
                    {
                        var targetUser = await dBContext.Users.Where(u => u.Id == notification.PeerId).Select(c=> new
                        {
                            PhoneNumber = c.PhoneNumber,
                            RawProfilePicUrl = c.ProfilePicUrl,
                            PicPrivacyLevel = c.PrivacySettings.ProfilePicPrivacy,
                            AmIInHisContacts = c.Contacts.Any(hisContact => hisContact.ContactUserId == notification.SenderId),
                            AmIExcludedFromPic = c.PrivacySettings.PrivacyExceptions.Any(e => e.ExcludedContactId == notification.SenderId && e.IsExcludedFromProfilePic),
                        }).FirstOrDefaultAsync(cancellationToken);

                        targetDisplayName = targetUser!.PhoneNumber!;
                        targetProfilePic = targetUser.RawProfilePicUrl.ResolveProfilePic(targetUser.PicPrivacyLevel,targetUser.AmIInHisContacts,targetUser.AmIExcludedFromPic);
                    }
                    else
                    {
                        var targetUser = await dBContext.Users.Where(u => u.Id == notification.SenderId).Select(c => new
                        {
                            PhoneNumber = c.PhoneNumber,
                            RawProfilePicUrl = c.ProfilePicUrl,
                            PicPrivacyLevel = c.PrivacySettings.ProfilePicPrivacy,
                            AmIInHisContacts = c.Contacts.Any(hisContact => hisContact.ContactUserId == participantId),
                            AmIExcludedFromPic = c.PrivacySettings.PrivacyExceptions.Any(e => e.ExcludedContactId == participantId && e.IsExcludedFromProfilePic),
                        }).FirstOrDefaultAsync(cancellationToken);

                        targetDisplayName = targetUser!.PhoneNumber!;
                        targetProfilePic = targetUser.RawProfilePicUrl.ResolveProfilePic(targetUser.PicPrivacyLevel, targetUser.AmIInHisContacts, targetUser.AmIExcludedFromPic);

                    }
                }


                var update = Builders<UserConversation>.Update
                    .Set(uc => uc.LastMessage, lastMessage)
                    .Set(uc => uc.UpdatedAt, DateTime.UtcNow)
                    .Inc(uc => uc.SyncVersion, 1)
                    .SetOnInsert(uc => uc.ChatId, notification.ChatId)
                    .SetOnInsert(uc => uc.UserId, participantId)
                    .SetOnInsert(uc => uc.ConversationType, notification.ConversationType)
                    .SetOnInsert(uc => uc.PeerId, currentPeerId)
                    .SetOnInsert(uc => uc.CreatedAt, DateTime.UtcNow)
                    .SetOnInsert(uc => uc.DisplayName, targetDisplayName)
                    .SetOnInsert(uc => uc.ProfilePicUrl, targetProfilePic);

                if (!isSender)
                {
                    update = update.Inc(uc => uc.UnreadCount, 1);
                    if (isMentioned) update = update.Inc(uc => uc.UnreadMentions, 1);
                }

                bulkOperations.Add(new UpdateOneModel<UserConversation>(filter, update) { IsUpsert = true });
            }

            if (bulkOperations.Any())
            {
                await userConversationCollection.BulkWriteAsync(bulkOperations, cancellationToken: cancellationToken);
            }
        }
    }
}
