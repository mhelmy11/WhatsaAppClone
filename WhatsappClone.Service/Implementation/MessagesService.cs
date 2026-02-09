using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Infrastructure.Repositories;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class MessagesService : IMessagesService
    {
        private readonly IMessageRepository messageRepo;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly ILogger<MessagesService> logger;
        private readonly IFileService fileService;
        private readonly IUserGroupRepository userGroupRepo;
        private readonly IGroupRepository groupRepo;

        public MessagesService(
                                IMessageRepository messageRepo,
                                IMapper mapper,
                                UserManager<AppUser> userManager,
                                ILogger<MessagesService> logger,
                                IFileService fileService,
                                IUserGroupRepository userGroupRepo,
                                IGroupRepository groupRepo)
        {
            this.messageRepo = messageRepo;
            this.mapper = mapper;
            this.userManager = userManager;
            this.logger = logger;
            this.fileService = fileService;
            this.userGroupRepo = userGroupRepo;
            this.groupRepo = groupRepo;
        }
        public List<Guid> GetMessagesOfGroupsIDs(List<Guid> GroupsIDs)
        {
            // retrieve the messages of groups by their group IDs using messageRepo
            // var messages = messageRepo.GetTableNoTracking()
            //     .Where(m => m.GroupId != null && GroupsIDs.Contains(m.GroupId.Value))
            //     .Select(m => m.Id)
            //     .ToList();

            // return messages;
            return new();

        }




        private static object ResolveContent(Message message)
        {
        //     if (message.IsSystemMessage)
        //     {
        //         return JsonSerializer.Deserialize<JsonElement>(message.Content);
        //     }
        //     return message.Content;
        // }
        // public List<ChatDTO> GetLasMessageOfGroupsIDs(List<Guid> GroupsIDs, string currentUserId)
        // {
        //     // retrieve the messages of groups by their group IDs using messageRepo
        //     var messages = messageRepo.GetTableNoTracking()
        //         .Where(m => m.GroupId != null && GroupsIDs.Contains(m.GroupId.Value))
        //         .GroupBy(m => m.GroupId!.Value)
        //         .Select(x => new ChatDTO
        //         {
        //             senderName = x.OrderByDescending(m => m.SentAt).First().Sender.FullName,
        //             senderId = x.OrderByDescending(m => m.SentAt).First().Sender.Id,
        //             chatName = x.First().Group!.Name,
        //             groupId = x.Key,
        //             // chatPic = x.First().Group!.GroupPictureUrl!,
        //             lastMessageContent = ResolveContent(x.OrderByDescending(m => m.SentAt).First()),
        //             lastMessageTime = x.OrderByDescending(m => m.SentAt).First().SentAt,
        //             // ImageCount = x.First().Attachments!.Where(a => a.Type == Attachment.Image).Count(),
        //             // VideoCount = x.First().Attachments!.Where(a => a.Type == Attachment.Video).Count(),
        //             isGroup = true,
        //             isMuted = x.First().Group!.ChatSettings!.Any(y => y.isMuted && y.UserId == currentUserId),
        //             isPinned = x.First().Group!.ChatSettings!.Any(y => y.IsPinned && y.UserId == currentUserId),
        //             isSystemMessage = x.OrderByDescending(m => m.SentAt).First().IsSystemMessage,
        //             messageStatus = x.First().MessageReadStatuses!.First(m => m.MessageId == x.First().Id).Status,

        //         })
        //         .OrderByDescending(x => x.lastMessageTime)
        //         .AsSplitQuery();



        //     logger.LogInformation(messages.ToQueryString());



        //     return messages.ToList(); ;

        return new();

        }






        public async Task AddSystemMessage(string? content, Guid groupId, string actorId, string messageType)
        {

            await messageRepo.AddAsync(new Message { GroupId = groupId, SenderId = actorId, Content = content, IsSystemMessage = true, MessageType = messageType });

        }




        public async Task<Message> SendGroupMessage(string senderId, Guid groupId, List<IFormFile>? attachmentsDTO, string content)
        {
            //check if user is a member of the group
            // var isUserInGroup = userGroupRepo.IsUserInGroup(senderId, groupId);

            // if (!isUserInGroup)
            // {
            //     throw new UnauthorizedAccessException("You are not a member of this group.");
            // }

            // var isAdmin = userGroupRepo.IsGroupAdmin(senderId, groupId);
            // // check group permissions
            // var isSendMessageAllowed = groupRepo.IsSendMessagesAllowed(groupId);



            // if (!isSendMessageAllowed && !isAdmin)
            // {
            //     throw new UnauthorizedAccessException("You are not allowed to send messages in this group.");
            // }
            // var transaction = messageRepo.BeginTransaction();
            // try
            // {

            //     var message = await messageRepo.AddAsync(new Message
            //     {
            //         GroupId = groupId,
            //         SenderId = senderId,
            //         Content = content,
            //         // Attachments = attachmentsDTO?.Select(a => new Attachments
            //         // {
            //         //     Type = a.ContentType switch
            //         //     {
            //         //         "image/jpeg" => Attachment.Image,
            //         //         "image/png" => Attachment.Image,
            //         //         "image/jpg" => Attachment.Image,
            //         //         "application/pdf" => Attachment.Document,
            //         //         "video/mp4" => Attachment.Video,
            //         //         _ => Attachment.None
            //         //     },
            //         //     Url = fileService.SaveFileAsync(a, $"Group_{groupId}").Result
            //         // }).ToList(),
            //         IsSystemMessage = false,
            //         MessageType = MessageTypeString.Text,
            //     });

            //     await transaction.CommitAsync();
            //     return message;
            // }
            // catch (Exception ex)
            // {
            //     await transaction.RollbackAsync();
            //     throw new Exception("An error occurred while sending the group message.", ex);
            // }
            return new();


        }

        public async Task EditGroupMessage(string actorId, Guid messageId, Guid groupId, string content)
        {

            // Retrieve the message from the database
            // var message = await messageRepo.GetTableNoTracking()
            //     .FirstAsync(m => m.Id == messageId && m.GroupId == groupId);
            // if (message == null)
            // {
            //     throw new Exception("Message not found.");
            // }

            // //if actor is the sender -> can edit only your message
            // if (!(message.SenderId == actorId))
            // {

            //     throw new UnauthorizedAccessException("you are not authorize to edit this message");

            // }
            // // Update the content of the message
            // message.Content = content;
            // message.IsEdit = true;
            // message.EditAt = DateTime.Now;
            // // Save the changes to the database
            // await messageRepo.UpdateAsync(message);


        }

        public async Task DeleteGroupMessage(string actorId, Guid messageId, Guid groupId)
        {

            // if (!userGroupRepo.IsUserInGroup(actorId, groupId))
            //     throw new UnauthorizedAccessException("you are not authorize to delete this message");


            // // get message
            // var message = await messageRepo.GetTableNoTracking()
            //           .FirstAsync(m => m.Id == messageId && m.GroupId == groupId);

            // if (message == null)
            // {
            //     throw new Exception("Message not found.");
            // }
            // //if actor is admin -> can delete any message else you can can delete only your message
            // if (!(message.SenderId == actorId) && !userGroupRepo.IsGroupAdmin(actorId, groupId))
            // {

            //     throw new UnauthorizedAccessException("you are not authorize to delete this message");

            // }


            // // Update the content of the message
            // message.IsDeleted = true;
            // var systemMessage = new
            // {

            //     type = "MESSAGE_DELETED",
            //     actorUserId = actorId
            // };

            // var content = JsonSerializer.Serialize(systemMessage);
            // message.Content = content;

            // await messageRepo.UpdateAsync(message);


        }

        public List<ChatDTO> GetLasMessageOfGroupsIDs(List<Guid> GroupsIDs, string currentUserId)
        {
            throw new NotImplementedException();
        }
    }
}
