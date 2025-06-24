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


        public Task<Group> CreateGroup(Group entity, string actorId, List<string> membersIDs);


        public Task RemoveMemberFromGroup(List<string> userIds, Guid groupId, string actorId);


        public Task<List<string>> AddListOfMembers(string actorId, Guid groupId, List<string> membersIDs);


        public Task LeaveGroup(string userId, Guid groupId);


        public Group GetGroupById(Guid groupId);
        public Task UpdateGroupPic(Group entity, string actorId, string? oldPic);
        public Task UpdateGroupName(Group entity, string actorId, string oldName);
        public Task UpdateGroupDescription(Group entity, string actorId);

    }
}
