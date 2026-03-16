using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Groups.Events
{
    public record GroupCreatedEvent(long actorId , string messageId, List<long> Participants) : INotification
    {
    }
}
