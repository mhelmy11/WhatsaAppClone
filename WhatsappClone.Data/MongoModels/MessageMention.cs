using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.MongoModels
{
    public class MessageMention : MongoBaseModel
    {

        [BsonElement("messageId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; }

        [BsonElement("chatId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ChatId { get; set; }

        [BsonElement("mentionedUserId")]
        public string MentionedUserId { get; set; }

        [BsonElement("mentionedAt")]
        public DateTime MentionedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isRead")]
        public bool IsRead { get; set; } = false;

        [BsonElement("readAt")]
        public DateTime? ReadAt { get; set; }
    }
}
