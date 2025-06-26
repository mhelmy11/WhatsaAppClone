using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class GroupPermissions
    {
        public Guid GroupId { get; set; }

        public int PermissionId { get; set; }


        public virtual Group Group { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
