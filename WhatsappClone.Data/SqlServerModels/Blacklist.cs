using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.SqlServerModels
{
    public class Blacklist
    {

        public string UserId { get; set; }
        public string BlockedUserId { get; set; }
        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;
        public virtual AppUser BlockedUser { get; set; }
        public virtual AppUser User { get; set; }
    }
}
