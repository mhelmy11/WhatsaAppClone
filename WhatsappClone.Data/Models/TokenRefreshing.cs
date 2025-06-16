using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class TokenRefreshing
    {
        [Key]
        public int Id { get; set; }

        // المفتاح الأجنبي لجدول المستخدمين
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; } // افترض أن اسم كلاس المستخدم هو ApplicationUser

        [Required]
        [MaxLength(256)]
        public string RefreshToken { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedDate { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        // خاصية مساعدة للتحقق بسهولة
        [NotMapped]
        public bool IsActive => !IsRevoked && ExpiryDate > DateTime.UtcNow;


    }
}
