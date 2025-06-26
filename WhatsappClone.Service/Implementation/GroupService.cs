using Microsoft.EntityFrameworkCore;
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

            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                await userGroupRepo.AddRangeAsync(membersIDs.Select(memberId => new UserGroup
                {
                    GroupId = groupId,
                    UserId = memberId,
                    Role = GroupRole.Member // Default role for new members
                }).ToList());


                // add system message for each member added
                foreach (var member in membersIDs)
                {
                    var systemMessage = new
                    {

                        type = "MEMBER_ADDED",
                        actorUserId = actorId,
                        targetUserIds = member
                    };

                    var content = JsonSerializer.Serialize(systemMessage);

                    await messagesService.AddSystemMessage(content, groupId, actorId, MessageType.AddMember);
                }

                await transaction.CommitAsync();
                return membersIDs;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error adding members to group", ex);
            }
        }

        public async Task<UserGroup> AddMemberToGroup(UserGroup entity)
        {
            return await userGroupRepo.AddAsync(entity);
        }

        public async Task<Group> CreateGroup(Group entity, string actorId, List<string>? membersIDs)
        {
            var transaction = userGroupRepo.BeginTransaction();
            try
            {

                // Add admin
                var result = await groupRepo.AddAsync(entity);
                await userGroupRepo.AddAsync(new UserGroup
                {
                    GroupId = entity.Id,
                    UserId = actorId,
                    Role = GroupRole.Admin // The creator is the admin of the group
                });
                var systemMessageCreate = new
                {

                    type = "CREATE_GROUP",
                    actorUserId = actorId,
                    groupName = entity.Name,
                };
                var contentCreate = JsonSerializer.Serialize(systemMessageCreate);

                await messagesService.AddSystemMessage(contentCreate, entity.Id, actorId, MessageType.GroupCreated);






                // add Members

                await userGroupRepo.AddRangeAsync(membersIDs.Select(memberId => new UserGroup
                {
                    GroupId = entity.Id,
                    UserId = memberId,
                    Role = GroupRole.Member // Default role for new members
                }).ToList());

                foreach (var member in membersIDs)
                {
                    var systemMessage = new
                    {

                        type = "MEMBER_ADDED",
                        actorUserId = actorId,
                        targetUserIds = member
                    };

                    var content = JsonSerializer.Serialize(systemMessage);

                    await messagesService.AddSystemMessage(content, entity.Id, actorId, MessageType.AddMember);
                }



                await transaction.CommitAsync();
                return result;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating group", ex);
            }

        }

        public Group GetGroupById(Guid groupId)
        {
            return groupRepo.GetTableNoTracking()
                .First(g => g.Id == groupId);

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

        public bool IsUserInGroup(string userId, Guid groupId)
        {
            return userGroupRepo.GetTableNoTracking().Any(ug => ug.UserId == userId && ug.GroupId == groupId);

        }

        public async Task LeaveGroup(string userId, Guid groupId)
        {
            var groupname = groupRepo.GetTableNoTracking().Where(g => g.Id == groupId).Select(g => g.Name).FirstOrDefault();
            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                var group = userGroupRepo.GetTableNoTracking()
                                         .Where(g => g.GroupId == groupId && userId == g.UserId)
                                         .ExecuteUpdate(p => p.SetProperty(g => g.isMember, false));



                // add system message for each member left
                var systemRemoveMessage = new
                {
                    type = "MEMBER_LEFT",
                    actorUserId = userId,
                    groupName = groupname,

                };

                var content = JsonSerializer.Serialize(systemRemoveMessage);
                await messagesService.AddSystemMessage(content, groupId, userId, MessageType.GroupMemberLeft);
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error removing members from group", ex);
            }
        }

        public async Task RemoveMemberFromGroup(List<string> userIDs, Guid groupId, string actorId)
        {
            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                var group = userGroupRepo.GetTableNoTracking()
                                         .Where(g => g.GroupId == groupId && userIDs.Contains(g.UserId))
                                         .ExecuteUpdate(p => p.SetProperty(g => g.isMember, false));


                // add system message for each member removed
                foreach (var member in userIDs)
                {
                    var systemRemoveMessage = new
                    {
                        type = "MEMBER_REMOVED",
                        actorUserId = actorId,
                        targetUserId = member
                    };

                    var content = JsonSerializer.Serialize(systemRemoveMessage);
                    await messagesService.AddSystemMessage(content, groupId, actorId, MessageType.RemoveMember);
                }
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error removing members from group", ex);
            }
        }

        public async Task UpdateGroupDescription(Group entity, string actorId)
        {
            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                await groupRepo.UpdateAsync(entity);

                // add system message for each member removed
                var systemRemoveMessage = new
                {
                    type = "GROUP_DESCRIPTION_UPDATED",
                    actorUserId = actorId,
                    newGroupDesc = entity.Description,
                };

                var content = JsonSerializer.Serialize(systemRemoveMessage);
                await messagesService.AddSystemMessage(content, entity.Id, actorId, MessageType.GroupDescriptionChanged);
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error while updating Group's Pic", ex);
            }
        }

        public async Task UpdateGroupName(Group entity, string actorId, string oldName)
        {
            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                await groupRepo.UpdateAsync(entity);

                // add system message for each member removed
                var systemRemoveMessage = new
                {
                    type = "GROUP_NAME_UPDATED",
                    actorUserId = actorId,
                    newGroupPic = entity.GroupPictureUrl,
                    oldGroupName = oldName
                };

                var content = JsonSerializer.Serialize(systemRemoveMessage);
                await messagesService.AddSystemMessage(content, entity.Id, actorId, MessageType.GroupNameChanged);
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error while updating Group's Pic", ex);
            }
        }

        public async Task UpdateGroupPic(Group entity, string actorId, string? oldPic)
        {

            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                await groupRepo.UpdateAsync(entity);

                // add system message for each member removed
                var systemRemoveMessage = new
                {
                    type = "GROUP_PICTURE_UPDATED",
                    actorUserId = actorId,
                    newGroupPic = entity.GroupPictureUrl,
                    oldGroupPic = oldPic
                };

                var content = JsonSerializer.Serialize(systemRemoveMessage);
                await messagesService.AddSystemMessage(content, entity.Id, actorId, MessageType.GroupPicChanged);
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error while updating Group's Pic", ex);
            }

        }




    }
}
