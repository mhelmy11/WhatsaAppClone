using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories
{
    public class ChatRepo : Repo<Chat>, IChat
    {
        private readonly Context context;

        public ChatRepo(Context context) : base(context)
        {
            this.context = context;

        }

        public async Task<List<Chat>> GetChatsAsync()
            => await Task.FromResult(new List<Chat>());
    }
}
