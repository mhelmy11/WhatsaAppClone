using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Service.Abstract
{
    public interface IMessagesService
    {


        public List<Guid> GetMessagesOfGroupsIDs(List<Guid> GroupsIDs);
        public List<ChatDTO> GetLasMessageOfGroupsIDs(List<Guid> GroupsIDs, string currentUserId);
        public Task AddSystemMessage(string? content, Guid groupId, string actorId, MessageType messageType);

        public Task<Message> AddMessage(string userId, string GroupName, Guid GroupId, string msg);

    }
}
