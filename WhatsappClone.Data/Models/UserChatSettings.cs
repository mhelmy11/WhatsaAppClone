using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class UserChatSettings
    {

        public int Id { get; set; } // Primary Key, IDENTITY

        public string UserId { get; set; } //Current User

        public string? ReceiverId { get; set; } //UserId or GroupId
        public Guid? GroupId { get; set; } //UserId or GroupId
        public bool IsPinned { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public bool isMuted { get; set; } = false;

        public bool IsArchived { get; set; } = false;

        public DateTime? MuteUntil { get; set; }

        public virtual AppUser User { get; set; }

        public virtual AppUser? Receiver { get; set; } // Nullable for direct messages
        public virtual Group? Group { get; set; } // Nullable for direct messages














    }
}
