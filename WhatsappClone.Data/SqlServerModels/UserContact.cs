using System;

namespace WhatsappClone.Data.Models
{
    /// <summary>
    /// User's contact list entry
    /// Stores saved contacts with optional custom display names
    /// </summary>
    public class UserContact
    {
        /// <summary>User ID who saved this contact (Primary Key)</summary>
        public string UserId { get; set; }

        /// <summary>Contact user ID (Primary Key)</summary>
        public string ContactId { get; set; }

        /// <summary>Custom display name for this contact (if null, use AppUser.FullName)</summary>
        public string? DisplayName { get; set; }

        /// <summary>Contact was added timestamp</summary>
        public DateTime AddedOn { get; set; } = DateTime.UtcNow;

        /// <summary>Is contact soft-deleted/removed</summary>
        public bool IsDeleted { get; set; } = false;


        /// <summary>User who saved this contact</summary>
        public virtual AppUser User { get; set; }

        /// <summary>The contact user</summary>
        public virtual AppUser Contact { get; set; }
    }
}
