using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WhatsappClone.Data.MongoModels
{
    public class MongoMessageMediaIndex
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("messageId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; }

        [BsonElement("chatId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ChatId { get; set; }

        [BsonElement("senderId")]
        public string SenderId { get; set; }

        [BsonElement("mediaType")]
        public string MediaType { get; set; } // image|video|audio|file|sticker

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
