using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.Models
{
    public class MessageReadStatus
    {


        public int MessageId { get; set; }
        public string UserId { get; set; }
        public MessageStatus Status { get; set; } // e.g., 1=Delivered, 2=Read

        public DateTime? StatusTimestamp { get; set; } = DateTime.UtcNow; // Time when the status was updated


        public virtual Message Message { get; set; } // Navigation property to the Message entity
        public virtual AppUser User { get; set; } // Navigation property to the AppUser entity


    }
}
