using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.SqlServerModels
{
    public class AppUser : IdentityUser
    {
        /// <summary>User's full display name in the system</summary>
        public string FullName { get; set; }

        /// <summary>User's status/bio message</summary>
        public string? About { get; set; } = "Hey there! I'm using WhatsappClone!";

        /// <summary>When the 'About' status was last updated</summary>
        public DateTime AboutUpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Profile picture URL (stored in wwwroot/Users/ProfilePics)</summary>
        public string? ProfilePicUrl { get; set; }

        /// <summary>Last seen timestamp in UTC</summary>
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        /// <summary>Currently online status (updated by SignalR connections)</summary>
        public bool IsActive { get; set; } = false;

        /// <summary>Account creation timestamp in UTC</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Flag for soft-deleted accounts</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Account status (Active, Suspended, Banned)</summary>
        public string AccountStatus { get; set; } = AccountStatusString.Active; // Active, Suspended, Banned

        // ============ PRIVACY SETTINGS ============

        /// <summary>Who cans see if i read their messages or not </summary>
        public bool AllowReadReceipts { get; set; } = true;
        /// <summary>Who can view my status/stories (Everyone, Contacts, Nobody)</summary>
        public string WhoCanSeeMyStory { get; set; } = "Everyone";

        /// <summary>Who can add me to groups (Everyone, Contacts, Nobody)</summary>
        public string WhoCanAddMeToGroups { get; set; } = "Everyone"; // Everyone, Contacts, Nobody

        /// <summary>Who can see my last seen (Everyone, Contacts, Nobody)</summary>
        public string WhoCanSeeMyLastSeen { get; set; } = "Everyone";

        /// <summary>Who can view my profile picture (Everyone, Contacts, Nobody)</summary>
        public string WhoCanViewProfilePic { get; set; } = "Everyone";

        // ============ PREFERENCES & SETTINGS ============

        /// <summary>User's preferred language (English, Spanish, French, etc.)</summary>
        public string PreferredLanguage { get; set; } = PreferredLanguageString.English;

        /// <summary>Theme preference (Light, Dark, SystemDefault)</summary>
        public string ThemePreference { get; set; } = ThemePreferenceString.Light; // Light, Dark, SystemDefault

        /// <summary>Mute all notifications flag</summary>
        public bool MuteAllNotifications { get; set; } = false;

        /// <summary>Mute call notifications</summary>
        public bool MuteCallNotifications { get; set; } = false;


        // ============ SQL SERVER RELATIONSHIPS ============

        /// <summary>Users blocked by this user</summary>
        public virtual ICollection<Blacklist> BlockedUsers { get; set; } = new HashSet<Blacklist>();

        /// <summary>Users who have blocked this user</summary>
        public virtual ICollection<Blacklist> BlockedByUsers { get; set; } = new HashSet<Blacklist>();

        /// <summary>User's contacts list</summary>
        public virtual ICollection<UserContact> Contacts { get; set; } = new HashSet<UserContact>();

        /// <summary>Users who have added this user as their contact</summary>
        public virtual ICollection<UserContact> ContactsOf { get; set; } = new HashSet<UserContact>();

        /// <summary>Groups this user is a member of</summary>
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();

        /// <summary>Chat settings for direct and group chats</summary>
        public virtual ICollection<UserChatSettings> ChatSettings { get; set; } = new HashSet<UserChatSettings>();

        /// <summary>Privacy exceptions for "My Contacts Except" rules</summary>
        public virtual ICollection<UserPrivacyException> PrivacyExceptions { get; set; } = new HashSet<UserPrivacyException>();

        /// <summary>Active WebSocket connections for real-time notifications</summary>
        public virtual ICollection<UserConnection> UserConnections { get; set; } = new HashSet<UserConnection>();

        /// <summary>JWT refresh tokens for authentication</summary>
        public virtual ICollection<RefreshTokenAudit> UserRefreshTokens { get; set; } = new HashSet<RefreshTokenAudit>();

        // ============ MONGODB COLLECTIONS ============
        // Note: These collections are NOT stored in this model but referenced by userId
        //
        // 1. messages - User's messages (sent/received)
        //    Query: db.messages.find({ $or: [{ senderId: userId }, { receiverId: userId }] })
        //    Indexes: chatId + createdAt, senderId + receiverId + createdAt
        //
        // 2. statuses - User's stories (24-hour expiry with TTL)
        //    Query: db.statuses.find({ userId: userId, isActive: true })
        //    Indexes: userId + createdAt, expiresAt (TTL)
        //
        // 3. message_mentions - @mentions tracking
        //    Query: db.message_mentions.find({ mentionedUserId: userId })
        //    Indexes: mentionedUserId + mentionedAt
        //
        // 4. starred_messages - Bookmarked messages
        //    Query: db.starred_messages.find({ userId: userId, isActive: true })
        //    Indexes: userId + starredAt
        //
        // 5. message_search - Full-text search index
        //    Query: db.message_search.find({ $text: { $search: term }, senderId: userId })
        //    Indexes: senderId + createdAt, text (full-text index)




    }
}
