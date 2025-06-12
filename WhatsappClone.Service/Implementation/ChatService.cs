using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;
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
        private readonly ChatRepo chatRepo;

        public ChatService(ChatRepo chatRepo)
        {
            this.chatRepo = chatRepo;

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
            var Chats = await chatRepo.GetTableNoTracking().Where(c => c.IsStarted && !c.isDeleted).ToListAsync();


            return Chats;
        }


    }
}
