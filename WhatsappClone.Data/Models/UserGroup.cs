using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class UserGroup
    {

        public Guid GroupId { get; set; } = Guid.NewGuid();

        public string UserId { get; set; }
        public string Role { get; set; } // 0 = Member, 1 = Admin
        public DateTime AddedOn { get; set; }

        public virtual Group Group { get; set; }

        public virtual AppUser User { get; set; }

    }
}
