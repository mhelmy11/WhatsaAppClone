using AutoMapper.Execution;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            // Check if the actor is an admin of the group and add members is allowed
            if (!userGroupRepo.IsUserInGroup(actorId, groupId) || !userGroupRepo.IsGroupAdmin(actorId, groupId) || !groupRepo.IsAddMembersAllowed(groupId))
            {
                throw new UnauthorizedAccessException("You are not authorized to add members to this group.");
            }
            var group = groupRepo.GetTableAsTracking().SingleOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                throw new Exception("Group not found");
            }

            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                await userGroupRepo.AddRangeAsync(membersIDs.Select(memberId => new UserGroup
                {
                    GroupId = groupId,
                    UserId = memberId,
                    Role = GroupRole.Member // Default role for new members
                }).ToList());

                group.MembersCount += membersIDs.Count();
                await groupRepo.UpdateAsync(group);

                // add system message for each member added
                foreach (var member in membersIDs)
                {
                    var systemMessage = new
                    {

                        type = GroupSystemMessage.MemeberAdded.ToString(),
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
            var group = groupRepo.GetTableAsTracking().SingleOrDefault(g => g.Id == entity.GroupId);
            if (group == null)
            {
                throw new Exception("Group not found");
            }
            var transaction = groupRepo.BeginTransaction();
            try
            {
                group.MembersCount++;
                await groupRepo.UpdateAsync(group);
                await transaction.CommitAsync();
                return await userGroupRepo.AddAsync(entity);
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                throw new Exception("Error while adding a member", ex);



            }
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

                    type = GroupSystemMessage.GroupCreated.ToString(),
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

            var groupfromDB = groupRepo.GetTableAsTracking().SingleOrDefault(g => g.Id == groupId);
            if (groupfromDB == null)
            {
                throw new Exception("Group Not Found");
            }
            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                var group = userGroupRepo.GetTableNoTracking()
                                         .Where(g => g.GroupId == groupId && userId == g.UserId)
                                         .ExecuteUpdate(p => p.SetProperty(g => g.isMember, false));

                groupfromDB.MembersCount--;
                await groupRepo.UpdateAsync(groupfromDB);



                // add system message for each member left
                var systemRemoveMessage = new
                {
                    type = GroupSystemMessage.MemberLeft.ToString(),
                    actorUserId = userId,
                    groupName = groupfromDB.Name,

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
        public async Task RevokeAdmin(string actorId, string userId, Guid groupId)
        {

            var members = userGroupRepo.GetTableAsTracking()
                .Where(ug => ug.GroupId == groupId && (ug.UserId == userId || ug.UserId == actorId)).ToList();

            var actor = members.FirstOrDefault(ug => ug.UserId == actorId);
            var revokedUser = members.FirstOrDefault(ug => ug.UserId == userId);

            if (actor == null || revokedUser == null || revokedUser.isMember == false)
            {
                throw new NullReferenceException("user not found");
            }

            if (actor.Role != GroupRole.Admin)
            {
                throw new UnauthorizedAccessException("you are not authorized to revoke this user");
            }

            if (revokedUser.Role == GroupRole.Member)
            {
                throw new Exception("user is already a member");
            }

            var transaction = userGroupRepo.BeginTransaction();
            try
            {

                revokedUser.Role = GroupRole.Member;

                var systemRevokeMessage = new
                {
                    type = GroupSystemMessage.MemberRevoked.ToString(),
                    actorUserId = actorId,
                    targetUserId = userId
                };

                var content = JsonSerializer.Serialize(systemRevokeMessage);
                await messagesService.AddSystemMessage(content, groupId, actorId, MessageType.Revoke);

                await transaction.CommitAsync();



            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                throw new Exception("Error while promoting the member", ex);

            }
        }
        public async Task PromoteToAdmin(string actorId, string userId, Guid groupId)
        {

            var members = userGroupRepo.GetTableAsTracking()
                .Where(ug => ug.GroupId == groupId && (ug.UserId == userId || ug.UserId == actorId)).ToList();

            var actor = members.FirstOrDefault(ug => ug.UserId == actorId);
            var promotedUser = members.FirstOrDefault(ug => ug.UserId == userId);

            if (actor == null || promotedUser == null || promotedUser.isMember == false)
            {
                throw new NullReferenceException("user not found");
            }

            if (actor.Role != GroupRole.Admin)
            {
                throw new UnauthorizedAccessException("you are not authorized to promote this user");
            }

            if (promotedUser.Role == GroupRole.Admin)
            {
                throw new Exception("user is already a admin");
            }

            var transaction = userGroupRepo.BeginTransaction();
            try
            {

                promotedUser.Role = GroupRole.Admin;

                var systemPromoteMessage = new
                {
                    type = GroupSystemMessage.MemberPromoted.ToString(),
                    actorUserId = actorId,
                    targetUserId = userId
                };

                var content = JsonSerializer.Serialize(systemPromoteMessage);
                await messagesService.AddSystemMessage(content, groupId, actorId, MessageType.Promote);

                await transaction.CommitAsync();



            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                throw new Exception("Error while promoting the member", ex);

            }
        }
        public async Task RemoveMemberFromGroup(List<string> userIDs, Guid groupId, string actorId)
        {

            //not admin or not in group
            if (!userGroupRepo.IsUserInGroup(actorId, groupId) || !userGroupRepo.IsGroupAdmin(actorId, groupId))
            {
                throw new UnauthorizedAccessException("You are not authorized to remove members from this group.");

            }
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
                        type = GroupSystemMessage.MemberRemoved.ToString(),
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
            // Check if the actor is an admin of the group and update group picture is allowed
            if (!userGroupRepo.IsUserInGroup(actorId, entity.Id) || !userGroupRepo.IsGroupAdmin(actorId, entity.Id) || !groupRepo.IsEditGroupAllowed(entity.Id))
            {
                throw new UnauthorizedAccessException("You are not authorized to update the group picture.");
            }
            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                await groupRepo.UpdateAsync(entity);

                // add system message for each member removed
                var systemRemoveMessage = new
                {
                    type = GroupSystemMessage.GroupDescriptionChanged.ToString(),
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
            // Check if the actor is an admin of the group and update group picture is allowed
            if (!userGroupRepo.IsUserInGroup(actorId, entity.Id) || !userGroupRepo.IsGroupAdmin(actorId, entity.Id) || !groupRepo.IsEditGroupAllowed(entity.Id))
            {
                throw new UnauthorizedAccessException("You are not authorized to update the group picture.");
            }
            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                await groupRepo.UpdateAsync(entity);

                // add system message for each member removed
                var systemRemoveMessage = new
                {
                    type = GroupSystemMessage.GroupNameChanged.ToString(),
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


            // Check if the actor is an admin of the group and update group picture is allowed
            if (!userGroupRepo.IsUserInGroup(actorId, entity.Id) || !userGroupRepo.IsGroupAdmin(actorId, entity.Id) || !groupRepo.IsEditGroupAllowed(entity.Id))
            {
                throw new UnauthorizedAccessException("You are not authorized to update the group picture.");
            }

            var transaction = userGroupRepo.BeginTransaction();
            try
            {
                await groupRepo.UpdateAsync(entity);

                // add system message for each member removed
                var systemRemoveMessage = new
                {
                    type = GroupSystemMessage.PicChanged.ToString(),
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

        public async Task EditGroupPermissions(string actorId, Group updatedGroup)
        {
            var originalGroup = groupRepo.GetTableNoTracking().First(g => g.Id == updatedGroup.Id);
            var transaction = userGroupRepo.BeginTransaction();
            if (!userGroupRepo.IsGroupAdmin(actorId, originalGroup.Id))
            {

                throw new UnauthorizedAccessException("you are not authorized to edit group permissions");

            }
            try
            {

                if (updatedGroup.AllowSendMessages != originalGroup.AllowSendMessages)
                {
                    var systemMessage = new
                    {
                        type = GroupSystemMessage.SendMessageAllowed.ToString(),
                        actorUserId = actorId,
                        isAllowed = updatedGroup.AllowSendMessages
                    };
                    var content = JsonSerializer.Serialize(systemMessage);
                    await messagesService.AddSystemMessage(content, originalGroup.Id, actorId, MessageType.GroupSettingsChanged);
                }
                if (updatedGroup.CanAddMembers != originalGroup.CanAddMembers)
                {
                    var systemMessage = new
                    {
                        type = GroupSystemMessage.AddMembersAllowed.ToString(),
                        actorUserId = actorId,
                        isAllowed = updatedGroup.CanAddMembers
                    };
                    var content = JsonSerializer.Serialize(systemMessage);
                    await messagesService.AddSystemMessage(content, originalGroup.Id, actorId, MessageType.GroupSettingsChanged);
                }
                if (updatedGroup.EditGroupSettings != originalGroup.EditGroupSettings)
                {
                    var systemMessage = new
                    {
                        type = GroupSystemMessage.EditGroupAllowed.ToString(),
                        actorUserId = actorId,
                        isAllowed = updatedGroup.EditGroupSettings
                    };
                    var content = JsonSerializer.Serialize(systemMessage);
                    await messagesService.AddSystemMessage(content, originalGroup.Id, actorId, MessageType.GroupSettingsChanged);
                }


                originalGroup.AllowSendMessages = updatedGroup.AllowSendMessages;
                originalGroup.CanAddMembers = updatedGroup.CanAddMembers;
                originalGroup.EditGroupSettings = updatedGroup.EditGroupSettings;
                await groupRepo.UpdateAsync(originalGroup);



                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();


                throw new Exception("Error while updating Group's Settings", ex);

            }



        }

        public async Task<string> GenerateInviteCode(Guid groupId, string actorId)
        {
            //check if actor is admin
            if (!userGroupRepo.IsGroupAdmin(actorId, groupId))
            {
                throw new UnauthorizedAccessException("you are not authorized to generate invite link");
            }
            //generate encoded code
            bool isUnique = true;
            string code;
            do
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var random = new Random();
                code = new string(Enumerable.Repeat(chars, 20)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                isUnique = groupRepo.GetTableNoTracking().Any(g => g.InviteCode == code && g.InviteCode != null);

            } while (isUnique);
            //add it to group table
            var group = groupRepo.GetTableAsTracking().First(g => g.Id == groupId);
            group.InviteCode = code;
            await groupRepo.UpdateAsync(group);

            return code;


        }

        public async Task JoinGroupViaInviteLink(string inviteCode, string actorId)
        {
            var group = groupRepo.GetTableAsTracking().FirstOrDefault(group => group.InviteCode == inviteCode);
            if (group == null)
            {

                throw new Exception("Invalid Invite Link");
            }
            var transaction = groupRepo.BeginTransaction();
            try
            {

                await userGroupRepo.AddAsync(new UserGroup
                {
                    Role = GroupRole.Member,
                    GroupId = group.Id,
                    UserId = actorId
                });

                group.MembersCount++;
                await groupRepo.UpdateAsync(group);
                //add system message
                var systemMessage = new
                {
                    type = GroupSystemMessage.InviteViaLink.ToString(),
                    actorUserId = actorId,
                };
                var content = JsonSerializer.Serialize(systemMessage);
                await messagesService.AddSystemMessage(content, group.Id, actorId, MessageType.GroupMemberJoinedViaLink);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                throw new Exception("Error while joining the group via invite link ", ex);

            }




        }

        public Group GetGroupIdByInviteCode(string inviteCode)
        {
            var group = groupRepo.GetTableNoTracking().FirstOrDefault(group => group.InviteCode == inviteCode);
            if (group == null)
            {

                throw new Exception("Invalid Invite Link");
            }
            return group;
        }

        public async Task TogglePinGroup(Guid groupId, string actorId, bool currentState)
        {
            var group = groupRepo.GetTableAsTracking().Include(g => g.ChatSettings).FirstOrDefault(g => g.Id == groupId);

            var transaction = groupRepo.BeginTransaction();


            if (group == null)
            {
                throw new Exception("Group not found");
            }
            if (!userGroupRepo.IsUserInGroup(actorId, groupId))
            {
                throw new Exception("Us is not a member in this group");

            }

            try
            {
                var chatSettings = group.ChatSettings.FirstOrDefault(g => g.GroupId == groupId && actorId == g.UserId);
                if (chatSettings == null)
                {
                    group.ChatSettings.Add(new UserChatSettings { IsPinned = true, PinnedAt = DateTime.Now, GroupId = groupId, UserId = actorId, ReceiverId = null });
                }

                else
                {
                    chatSettings.IsPinned = !currentState;
                    chatSettings.PinnedAt = DateTime.Now;
                }
                await groupRepo.UpdateAsync(group);
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                throw new Exception("error while updating pin state", ex);
            }
        }
    }
}
