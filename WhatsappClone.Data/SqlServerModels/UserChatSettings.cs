using System;

namespace WhatsappClone.Data.SqlServerModels
{
    /// <summary>
    /// User-specific chat settings and preferences
    /// Stores per-user configuration for each chat (direct or group)
    /// </summary>
    public class UserChatSettings
    {
        public int Id { get; set; }

        /// <summary>User who owns these settings</summary>
        public string UserId { get; set; }

        /// <summary>Chat ID (foreign key to Chat table)</summary>
        public Guid ChatId { get; set; }

        /// <summary>Is this chat pinned to top of list</summary>
        public bool IsPinned { get; set; } = false;

        /// <summary>When this chat was pinned</summary>
        public DateTime? PinnedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Is this chat archived (hidden from main list)</summary>
        public bool IsArchived { get; set; } = false;

        /// <summary>When this chat was archived</summary>
        public DateTime? ArchivedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Is this chat marked as favorite/starred</summary>
        public bool IsFavorite { get; set; } = false;

        /// <summary>When this chat was marked as favorite</summary>
        public DateTime? FavoritedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Is this chat soft-deleted for this user</summary>
        public bool IsDeleted { get; set; } = false;

        // ============ NOTIFICATION SETTINGS ============

        /// <summary>Mute all notifications for this chat</summary>
        public bool IsMuted { get; set; } = false;

        /// <summary>Mute notifications until this timestamp</summary>
        public DateTime? MutedUntil { get; set; } = DateTime.UtcNow;

        // ============ SQL SERVER RELATIONSHIPS ============

        /// <summary>User who owns these settings</summary>
        public virtual AppUser User { get; set; }

        /// <summary>Chat (direct or group)</summary>
        public virtual Chat Chat { get; set; }
    }
}

