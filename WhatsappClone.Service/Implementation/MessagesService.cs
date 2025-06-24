using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class MessagesService : IMessagesService
    {
        private readonly IMessage messageRepo;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public MessagesService(IMessage messageRepo, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.messageRepo = messageRepo;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        public List<Guid> GetMessagesOfGroupsIDs(List<Guid> GroupsIDs)
        {
            // retrieve the messages of groups by their group IDs using messageRepo
            var messages = messageRepo.GetTableNoTracking()
                .Where(m => m.GroupId != null && GroupsIDs.Contains(m.GroupId.Value))
                .Select(m => m.Id)
                .ToList();

            return messages;

        }
        public List<ChatDTO> GetLasMessageOfGroupsIDs(List<Guid> GroupsIDs, string currentUserId)
        {
            // retrieve the messages of groups by their group IDs using messageRepo
            var messages = messageRepo.GetTableNoTracking()
                .Where(m => m.GroupId != null && GroupsIDs.Contains(m.GroupId.Value))
                .GroupBy(m => m.GroupId!.Value)
                .Select(x => new ChatDTO
                {
                    SenderName = x.OrderByDescending(m => m.SentAt).First().Sender.FullName,
                    SenderId = x.OrderByDescending(m => m.SentAt).First().Sender.Id,
                    Name = x.First().Group!.Name,
                    Id = x.Key,
                    PicUrl = x.First().Group!.GroupPictureUrl!,
                    LastMessageContent = x.OrderByDescending(m => m.SentAt).First().Content,
                    SentAt = x.OrderByDescending(m => m.SentAt).First().SentAt,
                    ImageCount = x.First().Attachments!.Where(a => a.Type == Attachment.Image).Count(),
                    VideoCount = x.First().Attachments!.Where(a => a.Type == Attachment.Video).Count(),
                    isGroup = true,
                    isMuted = x.First().Group!.ChatSettings!.Any(y => y.isMuted && y.UserId == currentUserId),
                    isPinned = x.First().Group!.ChatSettings!.Any(y => y.IsPinned && y.UserId == currentUserId),
                    isSystemMessage = x.OrderByDescending(m => m.SentAt).First().IsSystemMessage,
                    MessageStatus = x.First().MessageReadStatuses!.Where(m => m.MessageId == x.First().Id).Select(v => v.Status).ToList()

                })
                .AsSplitQuery()
                .ToList();

            return messages;

        }

        public async Task<Message> AddMessage(string userId, string GroupName, Guid GroupId, string msg)
        {

            return await messageRepo.AddAsync(new Message { GroupId = GroupId, SenderId = userId, Content = msg, IsSystemMessage = true });
        }


        public async Task<Message> CreateGroupMessage(string actorUserId, string groupName)
        {
            var content = "{\r\n  \"type\": \"GROUP_CREATED\",\r\n  \"actorUserId\": \"user_id_of_creator\",\r\n  \"groupName\": \"The New Group Name\"\r\n}";
            // This method is not implemented in the original code.
            // You can implement it based on your requirements.
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task<Message> UpdateGroupMessage(string actorUserId)
        {
            var content = "{\r\n  \"type\": \"GROUP_UPDATED\",\r\n  \"actorUserId\": \"admin_user_id\"\r\n}";
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task AddSystemMessage(string? content, Guid groupId, string actorId, MessageType messageType)
        {

            await messageRepo.AddAsync(new Message { GroupId = groupId, SenderId = actorId, Content = content, IsSystemMessage = true, MessageType = messageType });

        }
        public async Task<Message> RemoveMemberMessage(string actorUserId, string targetUserId)
        {
            var content = "{\r\n  \"type\": \"MEMBER_REMOVED\",\r\n  \"actorUserId\": \"admin_user_id\",\r\n  \"targetUserId\": \"removed_member_id\"\r\n}";
            // This method is not implemented in the original code.
            // You can implement it based on your requirements.
            throw new NotImplementedException("This method is not implemented yet.");
        }

        public async Task<Message> LeaveGroupMessage(string actorUserId, string targetUserId)
        {
            var content = "{\r\n  \"type\": \"MEMBER_LEFT\",\r\n  \"actorUserId\": \"user_id_who_left\"\r\n}";
            // This method is not implemented in the original code.
            // You can implement it based on your requirements.
            throw new NotImplementedException("This method is not implemented yet.");
        }

        public async Task<Message> JoinGroupMessage(string actorUserId, string targetUserId)
        {
            var content = "{\r\n  \"type\": \"MEMBER_JOINED_BY_LINK\",\r\n  \"actorUserId\": \"user_id_who_joined\"\r\n}";
            // This method is not implemented in the original code.
            // You can implement it based on your requirements.
            throw new NotImplementedException("This method is not implemented yet.");
        }

    }
}
