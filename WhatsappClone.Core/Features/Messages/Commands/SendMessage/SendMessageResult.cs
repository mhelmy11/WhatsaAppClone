using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Messages.Commands.SendMessage
{
    public class SendMessageResult
    {
        public required string MessageId { get; set; }
        public SendMessageCommand? Message { get; set; }
    }
}
