using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;

namespace WhatsappClone.Service.Abstract
{
    public interface IChatService
    {
        public Task<List<Chat>> GetChatsAsync();
    }
}
