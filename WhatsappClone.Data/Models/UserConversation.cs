using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.Models
{

    [BsonCollection("user_conversations")]
    public class UserConversation : MongoBaseModel
    {

        [BsonElement("user_id")]
        public long UserId { get; set; }

        [BsonElement("chat_id")]
        public long ChatId { get; set; }

        [BsonElement("conversation_type")]
        public string ConversationType { get; set; }   // individual | group

        [BsonElement("peer_id")]
        public long PeerId { get; set; } // if PeerId == null -> group     else -> individual


        //[BsonElement("participants_cache")]
        //public Dictionary<long, string> ParticipantsCache { get; set; } = new();

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("display_name")]
        public string DisplayName { get; set; } // groupName | (ContactName | PhoneNumber)

        [BsonElement("profile_pic_url")]
        public string ProfilePicUrl { get; set; } = "default_avatar";


        // Last message preview
        [BsonElement("last_message")]
        public LastMessage LastMessage { get; set; }

        // User state
        [BsonElement("unread_count")]
        public int UnreadCount { get; set; }

        [BsonElement("unread_mentions")]
        public int UnreadMentions { get; set; }

        [BsonElement("last_read_message_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? LastReadMessageId { get; set; }

        [BsonElement("last_read_timestamp")]
        public DateTime? LastReadTimestamp { get; set; }

        [BsonElement("last_received_message_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? LastReceivedMessageId { get; set; }

        [BsonElement("last_played_message_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? LastPlayedMessageId { get; set; }

        // Settings
        [BsonElement("is_muted")]
        public bool IsMuted { get; set; } = false;

        [BsonElement("muted_until")]
        public DateTime? MutedUntil { get; set; }

        [BsonElement("is_pinned")]
        public bool IsPinned { get; set; } = false;

        [BsonElement("pinned_at")]
        public DateTime? PinnedAt { get; set; }

        [BsonElement("is_archived")]
        public bool IsArchived { get; set; } = false;

        [BsonElement("archived_at")]
        public DateTime? ArchivedAt { get; set; }

        //for Ordering
        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }

    public class LastMessage
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; }
        public LastMessageContent Content { get; set; }
        public string MessageType { get; set; }
        public long? SenderId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool? IsDeleted { get; set; }
        public string? MessageStatus { get; set; }
        public bool? IsMedia { get; set; }
    }

    public class LastMessageContent
    {
        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("media_type")]
        public string? MediaType { get; set; }

        [BsonElement("is_media")]
        public bool? IsMedia { get; set; }

        [BsonElement("media_count")]
        public int? MediaCount { get; set; } = 0;

        [BsonElement("caption")]
        public string? Caption { get; set; }

        [BsonElement("system_message")]

        public SystemMessage? SystemMessage { get; set; }
    }

    public class SystemMessage
    {
        [BsonElement("actor_id")]
        [BsonRepresentation(BsonType.Int64)]
        public long ActorId { get; set; }

        public string Type { get; set; } = SystemEventType.AddMember;
    }

    public static class LastMessageStatus
    {
        public const string Sent = "Sent";
        public const string Delivered = "Delivered";
        public const string Read = "Read";
    }
}

