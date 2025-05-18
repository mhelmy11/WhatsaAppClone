using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class UserGroup
    {

        public int GroupId { get; set; }

        public string UserId { get; set; }

        public virtual Group Group { get; set; }

        public virtual AppUser User { get; set; }

    }
}
