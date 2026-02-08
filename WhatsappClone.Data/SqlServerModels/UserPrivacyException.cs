using System;
using System.ComponentModel.DataAnnotations;

namespace WhatsappClone.Data.Models
{
    /// <summary>
    /// Privacy exceptions for "My Contacts Except" rules.
    /// Stores per-user exclusions by privacy type.
    /// </summary>
    public class UserPrivacyException
    {
        public int Id { get; set; }

        /// <summary>Owner user ID who configured the exception</summary>
        public string UserId { get; set; }

        /// <summary>Excluded contact user ID</summary>
        public string ExcludedUserId { get; set; }

        /// <summary>Privacy type (Status, ProfilePic, LastSeen, About)</summary>
        public string PrivacyType { get; set; }

        // ============ SQL SERVER RELATIONSHIPS ============

        /// <summary>User who owns this exception</summary>
        public virtual AppUser User { get; set; }

        /// <summary>Excluded contact</summary>
        public virtual AppUser ExcludedUser { get; set; }
    }
}
