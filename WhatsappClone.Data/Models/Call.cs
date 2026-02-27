using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappClone.Data.Models
{
    public class Call
    {
        public long Id { get; set; }

        public long CallerId { get; set; }
        [ForeignKey(nameof(CallerId))]
        public User Caller { get; set; }

        public long CalleeId { get; set; }
        [ForeignKey(nameof(CalleeId))]
        public User Callee { get; set; }

        public byte CallType { get; set; } // 1=audio, 2=video

        public byte CallDirection { get; set; } // 1=outgoing, 2=incoming

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? DurationSeconds { get; set; }

        public byte Status { get; set; } // 1=missed, 2=answered, 3=rejected, 4=cancelled, 5=failed

        public long? GroupCallId { get; set; }
        [ForeignKey(nameof(GroupCallId))]
        public Group GroupCall { get; set; }

        public bool IsInternational { get; set; } = false;

        [MaxLength(20)]
        public string CallQuality { get; set; } // excellent, good, poor

        public ICollection<CallParticipant> Participants { get; set; }
    }
}
