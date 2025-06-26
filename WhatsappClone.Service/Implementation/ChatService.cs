using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Infrastructure.Repositories;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class ChatService : IChatService
    {
        private readonly IChat chatRepo;
        private readonly IMessage messageRepo;
        private readonly IUserGroup userGroupRepo;


        public ChatService(IChat chatRepo, IMessage messageRepo, IUserGroup userGroupRepo)
        {
            this.chatRepo = chatRepo;
            this.messageRepo = messageRepo;
            this.userGroupRepo = userGroupRepo;
        }

        public Task<Chat> GetChatByIdAsync(int chatId)
        {
            var Chat = chatRepo.GetByIdAsync(chatId);

            return Chat;
        }

        public IQueryable<Chat> GetChatsAsQueryable()
        {
            return chatRepo.GetTableNoTracking().Include(x => x.Receiver).AsQueryable();
        }

        public IQueryable<Chat> FilterChatPaginatedQueryable(ChatOrderingEnum? orderingEnum, string? search)
        {
            var querable = GetChatsAsQueryable();
            if (search != null)
            {
                querable = querable.Where(x => x.Receiver!.UserName!.Contains(search) || x.Receiver!.PhoneNumber!.Contains(search));
            }
            switch (orderingEnum)
            {
                case ChatOrderingEnum.ChatId:
                    querable = querable.OrderBy(x => x.Id);
                    break;
                case ChatOrderingEnum.Name:
                    querable = querable.OrderBy(x => x.Receiver.UserName);
                    break;
                case ChatOrderingEnum.LastMessageTime:
                    querable = querable.OrderByDescending(x => x.LastMessageTime);
                    return querable;
            }

            return querable;
        }

        public async Task<List<Chat>> GetChatsAsync()
        {
            //add any additional logic here if needed
            var Chats = await chatRepo.GetTableNoTracking().Where(c => c.IsStarted && !c.isDeleted && !c.IsArchived).ToListAsync();
            return Chats;
        }


        public async Task<Chat> AddChatAsync(Chat chat)
        {
            var newChat = await chatRepo.AddAsync(chat);
            return newChat;
        }

        public List<ChatDTO> GetChatListOfCurrentUSer(string currenUserId)
        {
            var GroupsOfCurrentUser = userGroupRepo.GetTableNoTracking().Where(ug => ug.UserId == currenUserId).ToList(); // get all groups of current user
            return new List<ChatDTO>();

        }


    }
}
