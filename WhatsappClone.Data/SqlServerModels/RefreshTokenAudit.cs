using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappClone.Data.SqlServerModels
{
    /// <summary>
    /// Refresh token record for authentication
    /// Tracks issued refresh tokens with revocation and security info
    /// </summary>
    public class RefreshTokenAudit
    {
        public int Id { get; set; }

        /// <summary>User who owns this token</summary>
        public string UserId { get; set; }

        /// <summary>Refresh token value</summary>
        [Required]
        [MaxLength(500)]
        public string Token { get; set; }

        /// <summary>Token expiration timestamp</summary>
        [Required]
        public DateTime ExpiresAt { get; set; }

        /// <summary>When token was issued</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>IP address that requested this token</summary>
        [MaxLength(45)]
        public string? IpAddress { get; set; }

        /// <summary>User agent/device info that requested this token</summary>
        [MaxLength(500)]
        public string? UserAgent { get; set; }

        /// <summary>Is token revoked</summary>
        public bool IsRevoked { get; set; } = false;

        /// <summary>When token was revoked</summary>
        public DateTime? RevokedAt { get; set; }

        // ============ SQL SERVER RELATIONSHIPS ============

        /// <summary>User who owns this token</summary>
        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }

        // ============ COMPUTED PROPERTIES ============

        /// <summary>Is token currently active (not revoked and not expired)</summary>
        [NotMapped]
        public bool IsActive => !IsRevoked && ExpiresAt > DateTime.UtcNow;
    }
}
