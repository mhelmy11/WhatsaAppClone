using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Groups.Events;
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
            #region old
            //var userConversationCollection = mongoDBFactory.GetCollection<UserConversation>();
            //bool isMedia = notification.MessageType != MessageType.Text && notification.MessageType != MessageType.System;

            //string targetDisplayName = notification.ConversationType == ConversationType.Group? await dBContext.Groups.Where(g=>g.Id == notification.ChatId).Select(c=>  c.ProfilePicUrl).FirstAsync(cancellationToken) : string.Empty;
            //string targetProfilePic = notification.ConversationType == ConversationType.Group ? await dBContext.Groups.Where(g => g.Id == notification.ChatId).Select(c => c.Name).FirstAsync(cancellationToken) : string.Empty;
            //var lastMessage = new LastMessage
            //{
            //    MessageId = notification.MessageId,
            //    SenderId = notification.SenderId,
            //    Timestamp = notification.SentAt,
            //    IsDeleted = false,
            //    IsMedia = isMedia,
            //    Content = new LastMessageContent
            //    {
            //        Text = notification.TextContent,
            //        MediaType = isMedia ? notification.MessageType : null,
            //        IsMedia = isMedia,
            //        Caption = notification.Caption,
            //    }
            //};
            ////if(notification.MessageType == MessageType.System)
            ////{
            ////    lastMessage.Content = new LastMessageContent
            ////    {
            ////        Text = 
            ////    }
            ////}


            //var bulkOperations = new List<WriteModel<UserConversation>>();

            //foreach (var participantId in notification.Participants)
            //{
            //    bool isSender = participantId == notification.SenderId;
            //    bool isMentioned = notification.Mentions?.Contains(participantId) == true;

            //    long? currentPeerId = notification.ConversationType == ConversationType.Individual
            //        ? (isSender ? notification.PeerId : notification.SenderId)
            //        : null;

            //    var filter = Builders<UserConversation>.Filter.And(
            //        Builders<UserConversation>.Filter.Eq(uc => uc.ChatId, notification.ChatId),
            //        Builders<UserConversation>.Filter.Eq(uc => uc.UserId, participantId)
            //    );
            //    if(notification.ConversationType == ConversationType.Individual)
            //    {
            //        if (isSender)
            //        {
            //            var targetUser = await dBContext.Users.Where(u => u.Id == notification.PeerId).Select(c=> new
            //            {
            //                PhoneNumber = c.PhoneNumber,
            //                RawProfilePicUrl = c.ProfilePicUrl,
            //                PicPrivacyLevel = c.PrivacySettings.ProfilePicPrivacy,
            //                AmIInHisContacts = c.Contacts.Any(hisContact => hisContact.ContactUserId == notification.SenderId),
            //                AmIExcludedFromPic = c.PrivacySettings.PrivacyExceptions.Any(e => e.ExcludedContactId == notification.SenderId && e.IsExcludedFromProfilePic),
            //            }).FirstOrDefaultAsync(cancellationToken);

            //            targetDisplayName = targetUser!.PhoneNumber!;
            //            targetProfilePic = targetUser.RawProfilePicUrl.ResolveProfilePic(targetUser.PicPrivacyLevel,targetUser.AmIInHisContacts,targetUser.AmIExcludedFromPic);
            //        }
            //        else
            //        {
            //            var targetUser = await dBContext.Users.Where(u => u.Id == notification.SenderId).Select(c => new
            //            {
            //                PhoneNumber = c.PhoneNumber,
            //                RawProfilePicUrl = c.ProfilePicUrl,
            //                PicPrivacyLevel = c.PrivacySettings.ProfilePicPrivacy,
            //                AmIInHisContacts = c.Contacts.Any(hisContact => hisContact.ContactUserId == participantId),
            //                AmIExcludedFromPic = c.PrivacySettings.PrivacyExceptions.Any(e => e.ExcludedContactId == participantId && e.IsExcludedFromProfilePic),
            //            }).FirstOrDefaultAsync(cancellationToken);

            //            targetDisplayName = targetUser!.PhoneNumber!;
            //            targetProfilePic = targetUser.RawProfilePicUrl.ResolveProfilePic(targetUser.PicPrivacyLevel, targetUser.AmIInHisContacts, targetUser.AmIExcludedFromPic);

            //        }
            //    }


            //    var update = Builders<UserConversation>.Update
            //        .Set(uc => uc.LastMessage, lastMessage)
            //        .Set(uc => uc.UpdatedAt, DateTime.UtcNow)
            //        .Inc(uc => uc.SyncVersion, 1)
            //        .SetOnInsert(uc => uc.ChatId, notification.ChatId)
            //        .SetOnInsert(uc => uc.UserId, participantId)
            //        .SetOnInsert(uc => uc.ConversationType, notification.ConversationType)
            //        .SetOnInsert(uc => uc.PeerId, currentPeerId)
            //        .SetOnInsert(uc => uc.CreatedAt, DateTime.UtcNow)
            //        .SetOnInsert(uc => uc.DisplayName, targetDisplayName)
            //        .SetOnInsert(uc => uc.ProfilePicUrl, targetProfilePic);

            //    if (!isSender)
            //    {
            //        update = update.Inc(uc => uc.UnreadCount, 1);
            //        if (isMentioned) update = update.Inc(uc => uc.UnreadMentions, 1);
            //    }

            //    bulkOperations.Add(new UpdateOneModel<UserConversation>(filter, update) { IsUpsert = true });
            //}

            //if (bulkOperations.Any())
            //{
            //    await userConversationCollection.BulkWriteAsync(bulkOperations, cancellationToken: cancellationToken);
            //} 
            #endregion

            bool isMedia = notification.MessageType != MessageType.Text && notification.MessageType != MessageType.System;
            var userConversationCollection = mongoDBFactory.GetCollection<UserConversation>();

            var lastMessage = new LastMessage
            {
                MessageId = notification.MessageId,
                IsDeleted = false,
                IsMedia = isMedia ,
                MessageStatus = LastMessageStatus.Sent,
                SenderId = notification.SenderId,   
                Timestamp = notification.SentAt,
                MessageType = notification.MessageType, 
                Content = new LastMessageContent { 
                    Text = notification.TextContent,
                    Caption = notification.Caption ,
                    IsMedia = isMedia ,
                    MediaCount = notification.MediaCount ,
                    MediaType = notification.MessageType ,
                    SystemMessage = notification.MessageType == MessageType.System ? new SystemMessage { ActorId = notification.SenderId , Type = SystemEventType.AddMember} : null

                }
            };
            var bulkOperations = new List<WriteModel<UserConversation>>();
            var participantData = await GetParticipantDataBatch(notification, cancellationToken);
            foreach (var participantId in notification.Participants)
            {
                var operation = BuildUpdateOperation(notification, participantId, lastMessage, participantData);
                bulkOperations.Add(operation);
            }

            if (bulkOperations.Any())
            {
                await userConversationCollection.BulkWriteAsync(bulkOperations, cancellationToken: cancellationToken);
            }











        }

        private async Task<Dictionary<long, ParticipantData>> GetParticipantDataBatch(MessageCreatedEvent notification, CancellationToken cancellationToken)
        {
            var participantData = new Dictionary<long, ParticipantData>();

            if (notification.ConversationType == ConversationType.Individual)
            {
                var userIds = new[] { notification.SenderId, notification.PeerId };
                var users = await dBContext.Users
                    .Where(u => userIds.Contains(u.Id))
                    .Select(u => new
                    {
                        u.Id,
                        u.PhoneNumber,
                        u.ProfilePicUrl,
                        u.PrivacySettings.ProfilePicPrivacy,
                        Contacts = u.Contacts.Select(c => c.ContactUserId).ToList(),
                        PrivacyExceptions = u.PrivacySettings.PrivacyExceptions
                            .Where(e => e.IsExcludedFromProfilePic)
                            .Select(e => e.ExcludedContactId).ToList()
                    })
                    .ToListAsync(cancellationToken);

                foreach (var user in users)
                {
                    participantData[user.Id] = new ParticipantData
                    {
                        PhoneNumber = user.PhoneNumber,
                        RawProfilePicUrl = user.ProfilePicUrl,
                        PicPrivacyLevel = user.ProfilePicPrivacy,
                        Contacts = user.Contacts,
                        ExcludedFromPic = user.PrivacyExceptions
                    };
                }
            }
            else if (notification.ConversationType == ConversationType.Group)
            {
                var groupInfo = await GetGroupInfo(notification.ChatId, cancellationToken);
                foreach (var participantId in notification.Participants)
                {
                    participantData[participantId] = new ParticipantData
                    {
                        DisplayName = groupInfo.Name,
                        ProfilePicUrl = groupInfo.ProfilePicUrl
                    };
                }
            }

            return participantData;
        }

        private async Task<GroupInfo> GetGroupInfo(long groupId, CancellationToken cancellationToken)
        {
     

            var group = await dBContext.Groups
                .Where(g => g.Id == groupId)
                .Select(g => new GroupInfo
                {
                    Name = g.Name,
                    ProfilePicUrl = g.ProfilePicUrl
                })
                .FirstOrDefaultAsync(cancellationToken);
            return group;
        }

        private class ParticipantData
        {
            public string PhoneNumber { get; set; }
            public string RawProfilePicUrl { get; set; }
            public string PicPrivacyLevel { get; set; }
            public List<long> Contacts { get; set; } = new();
            public List<long> ExcludedFromPic { get; set; } = new();
            public string DisplayName { get; set; }
            public string ProfilePicUrl { get; set; }
        }

        private class GroupInfo
        {
            public string Name { get; set; }
            public string ProfilePicUrl { get; set; }
        }
        private UpdateOneModel<UserConversation> BuildUpdateOperation(
            MessageCreatedEvent notification,
            long participantId,
            LastMessage lastMessage,
            Dictionary<long, ParticipantData> participantData)
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

            string targetDisplayName = string.Empty;
            string targetProfilePic = string.Empty;

            if (notification.ConversationType == ConversationType.Individual && participantData.ContainsKey(participantId))
            {
                var data = participantData[participantId];
                var targetUserId = isSender ? notification.PeerId : notification.SenderId;

                if (participantData.TryGetValue(targetUserId.Value, out var targetData))
                {
                    targetDisplayName = targetData.PhoneNumber;
                    targetProfilePic = targetData.RawProfilePicUrl
                        .ResolveProfilePic(
                            targetData.PicPrivacyLevel,
                            targetData.Contacts.Contains(participantId),
                            targetData.ExcludedFromPic.Contains(participantId)
                        );
                }
            }
            else if (notification.ConversationType == ConversationType.Group && participantData.ContainsKey(participantId))
            {
                var data = participantData[participantId];
                targetDisplayName = data.DisplayName;
                targetProfilePic = data.ProfilePicUrl;
            }
            var update = Builders<UserConversation>.Update
                .Set(uc => uc.LastMessage, lastMessage)
                .Set(uc => uc.UpdatedAt, notification.SentAt)
                .SetOnInsert(uc => uc.ChatId, notification.ChatId)
                .SetOnInsert(uc => uc.UserId, participantId)
                .SetOnInsert(uc => uc.ConversationType, notification.ConversationType)
                .SetOnInsert(uc => uc.PeerId, currentPeerId)
                .SetOnInsert(uc => uc.CreatedAt, notification.SentAt)
                .SetOnInsert(uc => uc.DisplayName, targetDisplayName)
                .SetOnInsert(uc => uc.ProfilePicUrl, targetProfilePic);

            if (!isSender)
            {
                update = update.Inc(uc => uc.UnreadCount, 1);
                if (isMentioned && notification.ConversationType == ConversationType.Group) update = update.Inc(uc => uc.UnreadMentions, 1);
            }

            return new UpdateOneModel<UserConversation>(filter, update) { IsUpsert = true };
        }

      
    }
}
