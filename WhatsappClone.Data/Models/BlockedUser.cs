using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappClone.Data.Models
{
    public class BlockedUser
    {
        public long UserId { get; set; }


        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public long BlockedUserId { get; set; }


        [ForeignKey(nameof(BlockedUserId))]
        public User Blocked { get; set; }

        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;

    }
}
