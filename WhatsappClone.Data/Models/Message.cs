using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.Models
{

    [Obsolete("Message model is currently used for MongoDB storage.")]
    public class Message
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string Content { get; set; }

        public Guid? GroupId { get; set; } // Nullable for direct messages
        public string MessageType { get; set; }

        public string MessageStatus { get; set; }  // for groupid = null, so it is from contact not group

        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public bool IsEdit { get; set; } = false;
        public DateTime? EditAt { get; set; }
        public bool IsSystemMessage { get; set; } = false;



        // Navigation properties


        [JsonIgnore]
        public virtual AppUser Sender { get; set; }
        [JsonIgnore]
        public virtual AppUser? Receiver { get; set; }


        [ForeignKey("GroupId")]
        [JsonIgnore]
        public virtual Group? Group { get; set; } // Nullable for direct messages

        [JsonIgnore]
        public virtual ICollection<MessageReadStatus>? MessageReadStatuses { get; set; } = new HashSet<MessageReadStatus>();
        [JsonIgnore]
        public virtual ICollection<Attachments>? Attachments { get; set; } = new HashSet<Attachments>();
        [JsonIgnore]
        public virtual ICollection<MessageReaction>? MessageReactions { get; set; } = new HashSet<MessageReaction>();





    }
}
