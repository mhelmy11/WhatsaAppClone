using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<Chat>> GetChatsAsync()
        {
            //add any additional logic here if needed
            var Chats = await chatRepo.GetTableNoTracking().Where(c => c.IsStarted && !c.isDeleted).ToListAsync();


            return Chats;
        }
    }
}
