using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class Chat
    {


        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime LastMessageTime { get; set; }
        public bool IsStarted { get; set; } // Indicates if the chat has started
        public bool IsBlocked { get; set; } // Indicates if the chat is blocked
        public bool IsArchived { get; set; } // Indicates if the chat is archived
        public bool isDeleted { get; set; }
        public bool isPinned { get; set; }
        public bool isFavorite { get; set; }
        public int UnreadCount { get; set; } // Number of unread messages

        // Navigation properties
        public virtual AppUser Sender { get; set; }
        public virtual AppUser Receiver { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
