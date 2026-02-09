using System;
using System.Collections.Generic;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.SqlServerModels
{
    /// <summary>
    /// Chat model for direct and group conversations
    /// 
    /// Storage Strategy:
    /// - Chat metadata: SQL Server
    /// - Message content: MongoDB ('messages' collection)
    /// - Message read status: MongoDB ('readStatus' array in messages)
    /// - Reactions: MongoDB ('reactions' array in messages)
    /// </summary>
    public class Chat
    {
        /// <summary>Unique chat identifier (GUID to match MongoDB chatId)</summary>
        public Guid Id { get; set; }

        /// <summary>Chat type: "Direct" or "Group"</summary>
        public string ChatType { get; set; } = string.Empty; // "Direct" or "Group"

        // ============ DIRECT CHAT ONLY ============

        /// <summary>Sender/Initiator user ID (direct chats only)</summary>
        public string? SenderId { get; set; }

        /// <summary>Receiver user ID (direct chats only)</summary>
        public string? ReceiverId { get; set; }

        /// <summary>Group ID (group chats only)</summary>
        public Guid? GroupId { get; set; }

        // ============ LAST MESSAGE REFERENCE ============

        /// <summary>Last message ID from MongoDB (ObjectId) for fetching full message details</summary>
        public string? LastMessageId { get; set; }

        /// <summary>Last message timestamp (for sorting chat list)</summary>
        public DateTime? LastMessageTime { get; set; }

        /// <summary>Is chat soft-deleted</summary>
        public bool IsDeleted { get; set; } = false;

        // ============ SQL SERVER RELATIONSHIPS ============

        /// <summary>Message sender (direct chats)</summary>
        public virtual AppUser? Sender { get; set; }

        /// <summary>Message receiver (direct chats)</summary>
        public virtual AppUser? Receiver { get; set; }

        /// <summary>Group reference (group chats)</summary>
        public virtual Group? Group { get; set; }

        // ============ MONGODB REFERENCES ============
        // NOTE: Messages NOT stored here - see MongoDB 'messages' collection
        // Query: db.messages.find({ chatId: ObjectId(chatId) })
        // Includes: content, attachments, reactions, readStatus, etc.
    }
}
