using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Infrastructure.Repositories;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class ChatService : IChatService
    {
        private readonly IChat chatRepo;

        public ChatService(IChat chatRepo)
        {
            this.chatRepo = chatRepo;
        }
        public async Task<List<Chat>> GetChatsAsync()
        {
            //add any additional logic here if needed
            var Chats = await chatRepo.GetChatsAsync();

            return Chats;
        }
    }
}
