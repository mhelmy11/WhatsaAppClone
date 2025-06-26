using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class Permission
    {

        public int Id { get; set; }

        public bool CanAddMembers { get; set; } = true;

        public bool EditGroupSettings { get; set; } = true;

        public bool SendMessages { get; set; } = true;
        public bool ApproveMembers { get; set; } = false;
        public virtual ICollection<GroupPermissions> GroupPermissions { get; set; } = new List<GroupPermissions>();
    }
}
