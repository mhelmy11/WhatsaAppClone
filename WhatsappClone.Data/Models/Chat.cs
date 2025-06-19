using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.Models
{
    public class Chat
    {


        public int Id { get; set; }
        public string ChatName { get; set; } = "";
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        public int? GroupId { get; set; } = null;
        public string? LastMessageContent { get; set; } = "";

        public MessageType? LastMessageType { get; set; } // Type of the last message in the chat

        public DateTime? LastMessageTime { get; set; }  // Time of the last message in the chat
        public bool IsStarted { get; set; } = false; // Indicates if the chat has started
        public bool IsBlocked { get; set; } = false;// Indicates if the chat is blocked
        public bool IsArchived { get; set; } = false; // Indicates if the chat is archived
        public bool isDeleted { get; set; } = false;
        public bool isPinned { get; set; } = false;
        public bool isFavorite { get; set; } = false;
        public int UnreadCount { get; set; } = 0; // Number of unread messages = 0;

        // Navigation properties
        public virtual AppUser? Sender { get; set; }
        public virtual AppUser? Receiver { get; set; }

        public virtual Group? Group { get; set; }

        public virtual ICollection<Message>? Messages { get; set; } = new List<Message>();

    }
}
