using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class GroupService : IGroupService
    {
        private readonly IMessagesService messagesService;
        private readonly IMessageStatusesService messageStatusesService;
        private readonly IUserGroup userGroupRepo;
        private readonly IGroup groupRepo;

        public GroupService(IMessagesService messagesService, IMessageStatusesService messageStatusesService, IUserGroup userGroupRepo, IGroup groupRepo)
        {
            this.messagesService = messagesService;
            this.messageStatusesService = messageStatusesService;
            this.userGroupRepo = userGroupRepo;
            this.groupRepo = groupRepo;
        }

        public async Task<List<string>> AddListOfMembers(string actorId, Guid groupId, List<string> membersIDs)
        {

            userGroupRepo.BeginTransaction();
            try
            {
                await userGroupRepo.AddRangeAsync(membersIDs.Select(memberId => new UserGroup
                {
                    GroupId = groupId,
                    UserId = memberId,
                    Role = GroupRole.Member // Default role for new members
                }).ToList());


                // add system message for each member added
                var systemMessage = new
                {
                    type = "MEMBER_ADDED",
                    actorUserId = actorId, // Assuming the first member is the one who added others
                    targetUserIds = membersIDs
                };

                var content = JsonSerializer.Serialize(systemMessage);

                await messagesService.AddMemberMessage(content, groupId, actorId);

                userGroupRepo.Commit();
                return membersIDs;
            }
            catch (Exception ex)
            {
                userGroupRepo.RollBack();
                throw new Exception("Error adding members to group", ex);
            }
        }

        public async Task<UserGroup> AddMemberToGroup(UserGroup entity)
        {
            return await userGroupRepo.AddAsync(entity);
        }

        public async Task<Group> CreateGroup(Group entity)
        {
            return await groupRepo.AddAsync(entity);

        }

        public List<Guid> GetGroupIdsOfUser(string userId)
        {
            // retrieve the group IDs of a user by their userId using userGroupRepo
            var groupIds = userGroupRepo.GetTableNoTracking()
                .Where(ug => ug.UserId == userId)
                .Select(ug => ug.GroupId)
                .ToList();

            return groupIds;

        }

        public List<ChatDTO> GetGroupsOfUser(string userId)
        {





            var userGroupsIDs = GetGroupIdsOfUser(userId);                                   // retrieve the groups of a user by their userId
            var groupMessagesIDs = messagesService.GetMessagesOfGroupsIDs(userGroupsIDs);    // retrieve the messages of the groups by their IDs
            var messagesStatusesLookUp = messageStatusesService.GetMessageStatusesDict();    // messagesStatusesLookUp[1] -> return list of the recipient message status for the message with ID 1-> [user: 1 , status: seen , user: 2 , status: delivered... ]
            var lastMessagesOfGroups = messagesService.GetLasMessageOfGroupsIDs(groupMessagesIDs, userId);


            return lastMessagesOfGroups;
        }

        public async Task<bool> IsUserAdmin(string userId, Guid groupId)
        {
            return userGroupRepo.GetTableNoTracking().Any(ug => ug.UserId == userId && ug.GroupId == groupId && ug.Role == GroupRole.Admin);

        }

        public Task RemoveMemberFromGroup(string userId, Guid groupId)
        {
            var member = userGroupRepo.GetTableNoTracking()
                .FirstOrDefault(ug => ug.UserId == userId && ug.GroupId == groupId);
            if (member != null)
            {
                return userGroupRepo.DeleteAsync(member);
            }
            else
            {
                throw new Exception("Member not found in the group");
            }
        }
    }
}
