using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class MessageReaction
    {
        public long Id { get; set; }

        public Guid MessageId { get; set; }

        public string UserId { get; set; }

        public string Reaction { get; set; }


        public virtual Message Message { get; set; }
    }
}
