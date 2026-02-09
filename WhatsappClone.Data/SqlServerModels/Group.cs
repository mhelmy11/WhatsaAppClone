using System;
using System.Collections.Generic;

namespace WhatsappClone.Data.SqlServerModels
{
    /// <summary>
    /// Group model for group chat management
    /// 
    /// Storage Strategy:
    /// - Group metadata & settings: SQL Server
    /// - Group messages: MongoDB ('messages' collection with groupId)
    /// - Group members: SQL Server (UserGroup)
    /// </summary>
    public class Group
    {
        /// <summary>Unique group identifier (Primary Key)</summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>Group display name</summary>
        public string Name { get; set; }

        /// <summary>Group description/topic</summary>
        public string? Description { get; set; }

        /// <summary>Group icon/avatar URL (stored in wwwroot/Group_[GroupId])</summary>
        public string? IconUrl { get; set; }

        /// <summary>User ID of group creator</summary>
        public string CreatorId { get; set; }

        /// <summary>Invite code for joining (or null if disabled)</summary>
        public string? InviteCode { get; set; }

        // ============ MEMBER PERMISSIONS ============

        /// <summary>Can members add other members to the group</summary>
        public bool CanAddMembers { get; set; } = true;

        /// <summary>Can members edit group settings (name, description, icon)</summary>
        public bool CanEditGroupSettings { get; set; } = true;

        /// <summary>Can members send messages</summary>
        public bool CanSendMessages { get; set; } = true;

        /// <summary>New members require admin approval to join</summary>
        public bool RequireApprovalToJoin { get; set; } = false;

        // ============ TIMESTAMPS ============

        /// <summary>Group creation timestamp</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Last group modification timestamp (settings, members, etc.)</summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>When group was soft-deleted (if IsDeleted = true)</summary>
        public DateTime? DeletedAt { get; set; }

        // ============ SQL SERVER RELATIONSHIPS ============

        /// <summary>Group creator</summary>
        public virtual AppUser Creator { get; set; }

        /// <summary>Group members and their roles</summary>
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();

        // ============ MONGODB REFERENCES ============
        // NOTE: Messages NOT stored here - see MongoDB 'messages' collection
        // Query: db.messages.find({ groupId: ObjectId(groupId) })
        // Includes: content, attachments, reactions, readStatus, etc.
    }
}
