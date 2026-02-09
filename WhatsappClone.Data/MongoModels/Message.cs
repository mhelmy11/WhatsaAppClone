using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using WhatsappClone.Data.Enums;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.MongoModels
{

    public class Message : MongoBaseModel
    {

        [BsonElement("chatType")]
        public string ChatType { get; set; } // direct|group

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
        public string MessageType { get; set; } = MessageTypeString.Text; // text|image|video|audio|file|system|location|sticker

        [BsonElement("isSystemMessage")]
        public bool IsSystemMessage { get; set; } = false;

        [BsonElement("attachments")]
        public List<MessageAttachment> Attachments { get; set; } = new();

        [BsonElement("reactions")]
        public List<MessageReaction> Reactions { get; set; } = new();

        [BsonElement("readStatus")]
        public List<MessageReadStatus> ReadStatus { get; set; } = new();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("editedAt")]
        public DateTime? EditedAt { get; set; }

        [BsonElement("isEdited")]
        public bool IsEdited { get; set; } = false;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("deletedBy")]
        public string? DeletedBy { get; set; }

        [BsonElement("deleteType")]
        public string? DeleteType { get; set; } // soft|hard


        [BsonElement("poll")]
        public Poll? Poll { get; set; }

        [BsonElement("isDraft")]
        public bool IsDraft { get; set; } = false;

        [BsonElement("metadata")]
        public MessageMetadata Metadata { get; set; } = new();
    }

    public class MessageAttachment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        [BsonElement("type")]
        public string Type { get; set; } // image|video|audio|file

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("size")]
        public long Size { get; set; }

        [BsonElement("mime")]
        public string Mime { get; set; }

        [BsonElement("width")]
        public int Width { get; set; }

        [BsonElement("height")]
        public int Height { get; set; }

        [BsonElement("duration")]
        public int Duration { get; set; }
    }

    public class MessageMetadata
    {

        [BsonElement("replyToMessageId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReplyToMessageId { get; set; }

        [BsonElement("isForwarded")]
        public bool IsForwarded { get; set; } = false;

        [BsonElement("systemEventType")]
        public string? SystemEventType { get; set; }

        [BsonElement("actorId")]
        public string? ActorId { get; set; }

        [BsonElement("targetUserIds")]
        public List<string> TargetUserIds { get; set; } = new();


        [BsonElement("linkPreview")]
        public LinkPreview? LinkPreview { get; set; }

    }

    public class MessageReaction
    {
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("emoji")]
        public string Emoji { get; set; }

        [BsonElement("at")]
        public DateTime At { get; set; } = DateTime.UtcNow;
    }

    public class MessageReadStatus
    {
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = MessageStatusString.Pending;

        [BsonElement("at")]
        public DateTime At { get; set; } = DateTime.UtcNow;
    }

    public class LinkPreview
    {
        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("siteName")]
        public string SiteName { get; set; }

        [BsonElement("fetchedAt")]
        public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    }

    public class Poll
    {
        [BsonElement("question")]
        public string Question { get; set; }

        [BsonElement("options")]
        public List<PollOption> Options { get; set; } = new();

        [BsonElement("allowMultiple")]
        public bool AllowMultiple { get; set; } = false;

        [BsonElement("votes")]
        public List<PollVote> Votes { get; set; } = new();

        [BsonElement("closedAt")]
        public DateTime? ClosedAt { get; set; }
    }

    public class PollOption
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("count")]
        public int Count { get; set; } = 0;
    }

    public class PollVote
    {
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("optionIds")]
        [BsonRepresentation(BsonType.String)]
        public List<Guid> OptionIds { get; set; } = new();

        [BsonElement("votedAt")]
        public DateTime VotedAt { get; set; } = DateTime.UtcNow;
    }
}