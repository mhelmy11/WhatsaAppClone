using System;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.Models
{
    /// <summary>
    /// Group membership record
    /// Tracks user membership in groups with roles and approval status
    /// </summary>
    public class UserGroup
    {
        /// <summary>Group ID (Primary Key)</summary>
        public Guid GroupId { get; set; }

        /// <summary>User ID (Primary Key)</summary>
        public string UserId { get; set; }

        /// <summary>User's role in the group (Admin, Moderator, Member)</summary>
        public string Role { get; set; } = GroupRoleString.Member;  // Admin, Moderator, Member


        /// <summary>Is user approved to be in the group (if RequireApprovalToJoin is enabled)</summary>
        public bool IsApproved { get; set; } = true;

        /// <summary>Is user an active member of the group</summary>
        public bool IsMember { get; set; } = true;  // Only true if user is approved (if approval is required) and has not left the group
        // ============ TIMESTAMPS ============

        /// <summary>When user joined the group</summary>
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        /// <summary>When user left the group (if IsMember = false)</summary>
        public DateTime? LeftAt { get; set; }

        // ============ SQL SERVER RELATIONSHIPS ============

        /// <summary>The group</summary>
        public virtual Group Group { get; set; }

        /// <summary>The user</summary>
        public virtual AppUser User { get; set; }

    }
}
