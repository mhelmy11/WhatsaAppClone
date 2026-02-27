using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappClone.Data.Models
{
    public class Contact
    {
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long ContactUserId { get; set; }
        [ForeignKey(nameof(ContactUserId))]
        public User ContactUser { get; set; }
        public string ContactName { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
