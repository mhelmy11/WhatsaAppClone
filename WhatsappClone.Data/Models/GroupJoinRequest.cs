using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappClone.Data.Models
{
    public class GroupJoinRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RequestId { get; set; }

        public long GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, approved, rejected

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ProcessedAt { get; set; }

        public long? ProcessedBy { get; set; }
        [ForeignKey(nameof(ProcessedBy))]
        public User ProcessedByUser { get; set; }
    }
}
