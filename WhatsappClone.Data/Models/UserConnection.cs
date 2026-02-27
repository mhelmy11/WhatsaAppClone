using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WhatsappClone.Data.Models
{
    public class UserConnection
    {
        public long UserId { get; set; }
        public string ConnectionId { get; set; }
        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
    }
}
