using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;

namespace WhatsappClone.Data.Helpers
{
    public class MessageAttachmentsDTO
    {
        public Attachment Type { get; set; }

        public string Url { get; set; }
    }
}
