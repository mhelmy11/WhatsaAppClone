using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WhatsappClone.Data.MongoModels
{
    public class MongoStatus
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("content")]
        public string? Content { get; set; }

        [BsonElement("mediaUrl")]
        public string? MediaUrl { get; set; }

        [BsonElement("mediaType")]
        public string? MediaType { get; set; } // text|image|video

        [BsonElement("backgroundColor")]
        public string? BackgroundColor { get; set; }

        [BsonElement("fontStyle")]
        public string? FontStyle { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("expiresAt")]
        public DateTime ExpiresAt { get; set; }

        [BsonElement("viewCount")]
        public int ViewCount { get; set; } = 0;

        [BsonElement("views")]
        public List<StoryView> Views { get; set; } = new();

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }

    public class StoryView
    {
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("viewedAt")]
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    }
}
