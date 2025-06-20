using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class Group
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? GroupPictureUrl { get; set; }
        public string CreatorUserId { get; set; }
        public DateTime CreationDate { get; set; }

        public string CreatorId { get; set; }

        public virtual AppUser Creator { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();

        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
