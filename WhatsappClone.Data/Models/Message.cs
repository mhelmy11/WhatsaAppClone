using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.Models
{
    public class Message
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string Content { get; set; }

        public Guid? GroupId { get; set; } // Nullable for direct messages
        public MessageType MessageType { get; set; }

        public DateTime SentAt { get; set; }
        public bool IsDeleted { get; set; }
        public Attachment AttachmentId { get; set; }
        public string AttachmentUrl { get; set; }



        // Navigation properties

        public virtual AppUser Sender { get; set; }
        public virtual AppUser? Receiver { get; set; }

        public virtual Group? Group { get; set; } // Nullable for direct messages

        public virtual ICollection<MessageReadStatus> MessageReadStatuses { get; set; } = new HashSet<MessageReadStatus>();





    }
}
