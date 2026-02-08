using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.Models
{

    [Obsolete("Attachments model is currently unused. Embedded into Message Collection in MongoDB. Will be removed in future refactor.")]
    public class Attachments
    {
        public long Id { get; set; }
        // public Attachment Type { get; set; }

        public string Url { get; set; }

        public Guid MessageId { get; set; }


        public virtual Message Message { get; set; }
    }
}
