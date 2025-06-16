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

        public int Id { get; set; }
        public int ChatId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public MessageType MessageType { get; set; }

        public DateTime SentAt { get; set; }
        public bool IsDeleted { get; set; }
        public Attachment AttachmentId { get; set; }
        public string AttachmentUrl { get; set; }

        public Status Status { get; set; }



        // Navigation properties
        public virtual Chat Chat { get; set; }

        public virtual AppUser Sender { get; set; }
        public virtual AppUser Receiver { get; set; }



    }
}
