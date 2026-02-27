using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.Models
{

    [BsonCollection("messages")]
    public class Message : MongoBaseModel
    { 
       

        [BsonElement("chat_id")]
        [BsonRepresentation(BsonType.Int64)]

        public long? ChatId { get; set; }

        [BsonElement("sender_id")]
        [BsonRepresentation(BsonType.Int64)]

        public long SenderId { get; set; }

        [BsonElement("sender_device_id")]
        public string? SenderDeviceId { get; set; }

        [BsonElement("recipient_type")]
        public string RecipientType { get; set; } // "user", "group", "broadcast"

        [BsonElement("recipient_id")]
        [BsonRepresentation(BsonType.Int64)]

        public long? RecipientId { get; set; }

        [BsonElement("message_type")]
        public string MessageType { get; set; }

        [BsonElement("content")]
        public MessageContent Content { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [BsonElement("is_edited")]
        public bool? IsEdited { get; set; }

        [BsonElement("edited_at")]
        public DateTime? EditedAt { get; set; }

        [BsonElement("edited_by")]
        [BsonRepresentation(BsonType.Int64)]
        public long? EditedBy { get; set; }

        [BsonElement("is_deleted")]
        public bool? IsDeleted { get; set; } //deleted for everyone

        [BsonElement("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("deleted_by")]
        public long? DeletedBy { get; set; } // for group's Admin

        [BsonElement("delete_message_text")]
        public string? DeleteMessageText { get; set; }

        [BsonElement("delete_for")]
        public List<long>? DeletedFor { get; set; }

        [BsonElement("starred_for")]
        public List<long>? StarredFor { get; set; }

        [BsonElement("status")]
        public MessageStatus? Status { get; set; }

        [BsonElement("reactions")]
        public List<Reaction>? Reactions { get; set; } = new();

        [BsonElement("replies")]
        public List<Reply>? Replies { get; set; } = new();

        [BsonElement("reply_message_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReplyMessageId { get; set; }

        [BsonElement("disappear_duration")]
        public int? DisappearDuration { get; set; } // seconds

        [BsonElement("disappear_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? DisappearAt { get; set; }

        [BsonElement("mentions")]
        [BsonRepresentation(BsonType.Int64)]
        public List<long>? Mentions { get; set; } = new();// user_ids

    }

    public class MessageContent
    {
        // Text
        public string? Text { get; set; }
        public List<FormattedText>? FormattedText { get; set; }

        public SystemEvent? SystemEvent { get; set; }

        // Media
        public string? MediaUrl { get; set; }
        public string? MediaKey { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? FileName { get; set; }
        public int? FileSize { get; set; }
        public string? MimeType { get; set; }
        public int? Duration { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        // Location
        public Location? Location { get; set; }

        // Contact
        public ContactInfo? Contact { get; set; }

        // Poll
        public Poll? Poll { get; set; }

        // Payment
        public Payment? Payment { get; set; }

        // Sticker/GIF
        public string? StickerId { get; set; }
        public string? GifUrl { get; set; }

        // Caption
        public string? Caption { get; set; }

        // Link preview
        public string? Preview { get; set; }
    }
    public class SystemEvent
    {
        [BsonElement("event_type")]
        public string EventType { get; set; } // "member_added", "member_left", 
                                              // "group_created", "group_renamed",
                                              // "group_pic_changed", "admin_promoted",
                                              // "admin_demoted", "group_settings_changed"

        [BsonElement("actor_id")]
        [BsonRepresentation(BsonType.Int64)]
        public long ActorId { get; set; } 

        [BsonElement("target_ids")]
        [BsonRepresentation(BsonType.Int64)]
        public List<long>? TargetIds { get; set; } 

        [BsonElement("target_group_id")]
        [BsonRepresentation(BsonType.Int64)]
        public long? TargetGroupId { get; set; } 

        [BsonElement("old_value")]
        public string? OldValue { get; set; } 

        [BsonElement("new_value")]
        public string? NewValue { get; set; } 
    }

    public class FormattedText
    {
        public string? Type { get; set; } // "mention", "bold", "italic"
        public long? UserId { get; set; } // for mention
        public int? Offset { get; set; }
        public int? Length { get; set; }
    }

    public class Location
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

    public class ContactInfo
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Vcard { get; set; }
    }

    public class Poll
    {
        public string? Question { get; set; }
        public List<PollOption>? Options { get; set; }
        public bool? MultipleChoice { get; set; }
        public DateTime? EndTime { get; set; }
        public int? TotalVotes { get; set; }
        public List<long>? VotedUsers { get; set; }
    }

    public class PollOption
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public int? Votes { get; set; }
    }

    public class Payment
    {
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? TransactionId { get; set; }
    }

    public class MessageStatus
    {
        public DateTime? SentAt { get; set; }
        public List<Receipt>? DeliveredTo { get; set; } = new();
        public List<Receipt>? ReadBy { get; set; } = new();
        public List<Receipt>? PlayedBy { get; set; } = new();
        public int? ForwardedCount { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ForwardedFrom { get; set; }
    }

    public class Receipt
    {
        public string? UserId { get; set; }
        public string? DeviceId { get; set; }
        public DateTime? Timestamp { get; set; }
    }

    public class Reaction
    {
        public string? UserId { get; set; }
        public string? ReactionEmoji { get; set; }
        public DateTime? Timestamp { get; set; }
        public bool? Removed { get; set; }
    }

    public class Reply
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; }
        public string? UserId { get; set; }
        public string? Preview { get; set; }
        public DateTime? Timestamp { get; set; }
    }

}
