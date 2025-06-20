using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.Models
{
    public class Status
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }

        public StatusType ContentType { get; set; }


        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? Expiration { get; set; }

        public AppUser User { get; set; }


    }
}
