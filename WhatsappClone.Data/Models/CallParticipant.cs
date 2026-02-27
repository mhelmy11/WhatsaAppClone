using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappClone.Data.Models
{
    public class CallParticipant
    {
        public long CallId { get; set; }
        [ForeignKey(nameof(CallId))]
        public Call Call { get; set; }

        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public DateTime? JoinedAt { get; set; }

        public DateTime? LeftAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? DurationSeconds { get; private set; }
    }
}
