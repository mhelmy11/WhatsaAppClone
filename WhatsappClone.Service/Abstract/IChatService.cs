using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure;

namespace WhatsappClone.Service.Abstract
{
    public interface IChatService
    {
        public Task<List<Chat>> GetChatsAsync();
        public Task<Chat> GetChatByIdAsync(int chatId);

        public IQueryable<Chat> GetChatsAsQueryable();
        public Task<Chat> AddChatAsync(Chat chat);

        public List<ChatDTO> GetChatListOfCurrentUSer(string currenUserId);


        public IQueryable<Chat> FilterChatPaginatedQueryable(ChatOrderingEnum? orderingEnum, string? search);
    }
}
