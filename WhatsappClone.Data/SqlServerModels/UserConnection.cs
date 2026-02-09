using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.SqlServerModels
{
    public class UserConnection
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

        public virtual AppUser User { get; set; }
    }
}
