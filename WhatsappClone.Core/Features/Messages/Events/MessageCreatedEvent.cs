using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Messages.Events
{
    public record MessageCreatedEvent(
     string MessageId,
     long ChatId,
     long SenderId,
     long PeerId,
     string ConversationType,
     string MessageType,
     string? TextContent,
     string? Caption,
     List<long> Participants, 
     List<long>? Mentions, 
     DateTime SentAt
 ) : INotification;
}
