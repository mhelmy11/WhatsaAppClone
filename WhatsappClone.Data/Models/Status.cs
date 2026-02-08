using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    /// <summary>
    /// Status/Story model stored in MongoDB (NoSQL)
    /// Stories expire after 24 hours automatically
    /// </summary>
    [BsonIgnoreExtraElements]

    [Obsolete("Status model is currently used for MongoDB storage.")]
    public class Status
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("statusId")]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid StatusId { get; set; } = Guid.NewGuid();

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("content")]
        public string? Content { get; set; }

        [BsonElement("mediaUrl")]
        public string? MediaUrl { get; set; }

        [BsonElement("mediaType")]
        public string? MediaType { get; set; } // text, image, video

        [BsonElement("backgroundColor")]
        public string? BackgroundColor { get; set; }

        [BsonElement("fontStyle")]
        public string? FontStyle { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("expiresAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(24);

        [BsonElement("viewCount")]
        public int ViewCount { get; set; } = 0;

        [BsonElement("views")]
        public List<StatusView> Views { get; set; } = new List<StatusView>();

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        // Navigation property for SQL User reference (not stored in MongoDB)
        [BsonIgnore]
        public AppUser User { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class StatusView
    {
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("viewedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    }
}
