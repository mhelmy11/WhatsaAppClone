using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.MongoModels
{
    public class MessageSearch : MongoBaseModel
    {

        [BsonElement("messageId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; }

        [BsonElement("chatId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ChatId { get; set; }

        [BsonElement("senderId")]
        public string SenderId { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("tokens")]
        public List<string> Tokens { get; set; } = new();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
