using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Service.Abstract
{
    public interface IGroupService
    {
        public List<ChatDTO> GetGroupsOfUser(string userId);

        public Task<bool> IsUserAdmin(string userId, Guid groupId);

        public List<Guid> GetGroupIdsOfUser(string userId);

        public Task<UserGroup> AddMemberToGroup(UserGroup entity);


        public Task<Group> CreateGroup(Group entity);


        public Task RemoveMemberFromGroup(string userId, Guid groupId);


        public Task<List<string>> AddListOfMembers(string actorId, Guid groupId, List<string> membersIDs);
    }
}
