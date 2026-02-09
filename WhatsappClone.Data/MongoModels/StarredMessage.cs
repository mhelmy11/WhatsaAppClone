using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.MongoModels
{
    public class StarredMessage : MongoBaseModel
    {

        [BsonElement("messageId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; }

        [BsonElement("chatId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ChatId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("starredAt")]
        public DateTime StarredAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }
}
