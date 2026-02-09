using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.MongoModels
{
    public class MessageThread : MongoBaseModel
    {
        [BsonElement("chatId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ChatId { get; set; }

        [BsonElement("rootMessageId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RootMessageId { get; set; }

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("messageCount")]
        public int MessageCount { get; set; } = 0;
    }
}
