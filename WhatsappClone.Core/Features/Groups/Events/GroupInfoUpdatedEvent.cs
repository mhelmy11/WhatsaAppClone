using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Groups.Events
{
    public record GroupInfoUpdatedEvent
    {
        public long GroupId { get; init; }
        public string? GroupName { get; init; }
        public string? GroupDescription { get; init; }
        public string? GroupPic { get; init; }
        public long UpdatedByUserId { get; init; }
        public DateTime UpdatedAt { get; init; }


    }
}
