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
        public Guid Id { get; set; } =  Guid.NewGuid();

        public string ChatType { get; set; } = string.Empty; // "Direct" or "Group"

        // ============ DIRECT CHAT ONLY ============

        public string? SenderId { get; set; }

        public string? ReceiverId { get; set; }

        public Guid? GroupId { get; set; }

        // ============ LAST MESSAGE REFERENCE ============

        public string? LastMessageId { get; set; }

        public DateTime? LastMessageTime { get; set; }

        public bool IsDeleted { get; set; } = false;

        // ============ SQL SERVER RELATIONSHIPS ============

        public virtual AppUser? Sender { get; set; }

        public virtual AppUser? Receiver { get; set; }

        public virtual Group? Group { get; set; }

        // ============ MONGODB REFERENCES ============
        // NOTE: Messages NOT stored here - see MongoDB 'messages' collection
        // Query: db.messages.find({ chatId: ObjectId(chatId) })
        // Includes: content, attachments, reactions, readStatus, etc.
    }
}
