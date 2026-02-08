using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WhatsappClone.Data.MongoModels
{
    public class MongoScheduledMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("chatId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ChatId { get; set; }

        [BsonElement("senderId")]
        public string SenderId { get; set; }

        [BsonElement("receiverId")]
        public string? ReceiverId { get; set; }

        [BsonElement("groupId")]
        [BsonRepresentation(BsonType.String)]
        public Guid? GroupId { get; set; }

        [BsonElement("content")]
        public string? Content { get; set; }

        [BsonElement("messageType")]
        public string MessageType { get; set; } // text|image|video|audio|file

        [BsonElement("attachments")]
        public List<MessageAttachment> Attachments { get; set; } = new();

        [BsonElement("scheduledAt")]
        public DateTime ScheduledAt { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "pending"; // pending|sent|failed|canceled

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
