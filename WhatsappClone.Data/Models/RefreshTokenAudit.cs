using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappClone.Data.Models
{
    /// <summary>
    /// Refresh token record for authentication
    /// Tracks issued refresh tokens with revocation and security info
    /// </summary>
    public class RefreshTokenAudit
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Token { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        [MaxLength(500)]
        public string? UserAgent { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedAt { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }


        // COMPUTED 
        [NotMapped]
        public bool IsActive => !IsRevoked && ExpiresAt > DateTime.UtcNow;
    }
}
