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


        [BsonElement("recipient_type")]
        public string RecipientType { get; set; } // "individual", "group", "broadcast"

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

        [BsonElement("is_deleted_forEveryone")]
        public bool? IsDeletedForEveryone { get; set; } = false;

        [BsonElement("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("delete_for")]
        public List<long>? DeletedFor { get; set; } // delete for me option

        [BsonElement("starred_for")]
        public List<long>? StarredFor { get; set; }

        [BsonElement("status")]
        public MessageStatus? Status { get; set; }

        [BsonElement("reactions")]
        public List<Reaction>? Reactions { get; set; } = new();

        [BsonElement("reply_message_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReplyMessageId { get; set; }

        

    }

    public class MessageContent
    {
        // Text
        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("caption")]

        public string? Caption { get; set; }


        // For Mentions and  Bold,Italic Text 
        [BsonElement("sormatted_text")]

        public List<FormattedText>? FormattedText { get; set; }


        //for system Messages (you added ahmed , this message was deleted by.. ,  you created this group , you removed ahmed ,....)
        [BsonElement("system_event")]

        public SystemEvent? SystemEvent { get; set; }

        // Media
        [BsonElement("media")]

        public List<MediaMessage>? Media { get; set; } = new();

        // Location
        [BsonElement("location")]

        public Location? Location { get; set; }

        // Contact
        [BsonElement("contact")]

        public ContactInfo? Contact { get; set; }

        // Poll
        [BsonElement("poll")]

        public Poll? Poll { get; set; }


    }
    public class SystemEvent
    {
        [BsonElement("event_type")]
        public string EventType { get; set; } 

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
        [BsonElement("type")]
        public string? Type { get; set; } // "mention", "bold", "italic"

        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.Int64)]
        public long? UserId { get; set; } // for mention

        [BsonElement("offset")]

        public int? Offset { get; set; }

        [BsonElement("length")]

        public int? Length { get; set; }
    }

    public class Location
    {
        [BsonElement("lat")]

        public double? Lat { get; set; }
        [BsonElement("lng")]

        public double? Lng { get; set; }
        [BsonElement("name")]

        public string? Name { get; set; }
        [BsonElement("address")]

        public string? Address { get; set; }
    }

    public class ContactInfo
    {
        [BsonElement("name")]

        public string? Name { get; set; }
        [BsonElement("phone")]

        public string? Phone { get; set; }

    }

    public class Poll
    {
        [BsonElement("question")]

        public string? Question { get; set; }
        [BsonElement("options")]

        public List<PollOption>? Options { get; set; }
        [BsonElement("multiple_choice")]

        public bool? MultipleChoice { get; set; }
        [BsonElement("end_time")]

        public DateTime? EndTime { get; set; }
        [BsonElement("total_votes")]

        public int? TotalVotes { get; set; }
        [BsonElement("voted_users")]
        [BsonRepresentation(BsonType.Int64)]

        public List<long>? VotedUsers { get; set; }
    }

    public class PollOption
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("text")]
        public string? Text { get; set; }
        [BsonElement("votes_count")]

        public int? Votes { get; set; }
    }

     public class MessageStatus
    {

        [BsonElement("sent_at")]

        public DateTime? SentAt { get; set; }
        [BsonElement("delivered_to")]

        public List<Receipt>? DeliveredTo { get; set; } = new();
        [BsonElement("read_by")]

        public List<Receipt>? ReadBy { get; set; } = new(); 
        [BsonElement("played_by")]

        public List<Receipt>? PlayedBy { get; set; } = new();
    }

    public class Receipt
    {
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.Int64)]
        public long? UserId { get; set; }
        [BsonElement("timestamp")]

        public DateTime? Timestamp { get; set; }
    }

    public class Reaction
    {
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.Int64)]
        public long? UserId { get; set; }

        [BsonElement("reaction_emoji")]
        public string? ReactionEmoji { get; set; }


        [BsonElement("timestamp")]
        public DateTime? Timestamp { get; set; }

    }

    public class Reply
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; }
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.Int64)]
        public long? UserId { get; set; }
        [BsonElement("preview")]
        public string? Preview { get; set; }
        [BsonElement("timestamp")]

        public DateTime? Timestamp { get; set; }
    }
    public class MediaMessage
    {
        [BsonElement("media_url")]

        public string? MediaUrl { get; set; }
        [BsonElement("thumbnail")]

        public string? ThumbnailUrl { get; set; }
        [BsonElement("file_name")]

        public string? FileName { get; set; }
        [BsonElement("file_size")]

        public int? FileSize { get; set; }
        [BsonElement("mime_type")]

        public string? MimeType { get; set; }
        [BsonElement("duration")]

        public int? Duration { get; set; }
        [BsonElement("width")]

        public int? Width { get; set; }
        [BsonElement("height")]

        public int? Height { get; set; }

        // Sticker/GIF   (Webp)
        [BsonElement("sticker")]

        public string? StickerId { get; set; }
        [BsonElement("gif")]

        public string? GifUrl { get; set; }


        // Link preview
        [BsonElement("preview")]

        public string? Preview { get; set; }
    }
    public static class MessageType
    {
        public const string System = "System";
        public const string Text = "Text";
        public const string Video = "Video";
        public const string Image = "Image";
        public const string Audio = "Audio";
        public const string File = "File";
        public const string Document = "Document";
        public const string Sticker = "Sticker";
        public const string Gif = "Gif";
        public const string Contact = "Contact";
        public const string Poll = "Poll";
        public const string Location = "Location";
    }
    public static class SystemEventType
    {
        public const string AddMember = "AddMember";
        public const string RemoveMember = "RemoveMember";
        public const string LeftGroup = "LeftGroup";
        public const string EditGroupSettings = "EditGroupSettings";
        public const string SendNewMessages = "SendNewMessages";
        public const string AddOtherMembers = "AddOtherMembers";

    }

    public static class FormattedTextType
    {
        public const string Bold = "Bold";
        public const string Italic = "Italic";
        public const string Mention = "Mention";
        public const string Strikethrough = "Strikethrough";
    }

}
