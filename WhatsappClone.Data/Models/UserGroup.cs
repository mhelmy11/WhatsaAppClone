using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.Models
{
    public class UserGroup
    {

        public Guid GroupId { get; set; }

        public string UserId { get; set; }
        public GroupRole Role { get; set; } // 0 = Member, 1 = Admin
        public DateTime AddedOn { get; set; } = DateTime.Now;
        public DateTime? LeftAt { get; set; }


        public bool isMember { get; set; } = true;


        [ForeignKey("GroupId")]

        public virtual Group Group { get; set; }

        public virtual AppUser User { get; set; }

    }
}
