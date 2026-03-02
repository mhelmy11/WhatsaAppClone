using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.Models
{
    [BsonCollection("presences")]
    public class Presence : MongoBaseModel
    {

        [BsonElement("user_id")]
        public long UserId { get; set; }

        [BsonElement("online")]
        public bool Online { get; set; } = true;

        [BsonElement("last_seen")]
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        [BsonElement("last_active")]
        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        [BsonElement("active_status")]
        public string ActiveStatus { get; set; } = "active";

        [BsonElement("typing_in")]
        public List<TypingIndicator> TypingIn { get; set; } = new();

        [BsonElement("devices")]
        public List<PresenceDevice> Devices { get; set; } = new();

        [BsonElement("last_seen_privacy")]
        public string LastSeenPrivacy { get; set; } = "everyone"; // everyone, contacts, nobody

        [BsonElement("onlineStatus_privacy")]

        public string OnlineStatusPrivacy { get; set; } = "everyone"; // everyone, contacts, nobody

        [BsonElement("lastSeen_MyContactsExcept")]
        public List<long>? CanSeeMyLastSeenMyContactsExcept { get; set; } = new();


        [BsonElement("profile_pic_privacy")]
        public string ProfilePicPrivacy { get; set; } = "everyone"; // everyone, contacts, nobody

        [BsonElement("profilePic_MyContactsExcept")]
        public List<long>? CanViewProfilePicMyContactsExcept { get; set; } = new();


        [BsonElement("status_privacy")]
        public string StatusPrivacy { get; set; } = "contacts"; //contacts

        [BsonElement("status_MyContactsExcept")]
        public List<long>? CanViewStatusMyContactsExcept { get; set; } = new();

        [BsonElement("status_OnlyShareWith")]
        public List<long>? CanViewStatusOnlyShareWith { get; set; } = new();

        [BsonElement("read_receipts")]
        public bool ReadReceipts { get; set; } = true;

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public class TypingIndicator
        {
            public string ChatId { get; set; }
            public DateTime StartedAt { get; set; }
        }

        public class PresenceDevice
        {
            public string DeviceId { get; set; }
            public string Platform { get; set; }
            public DateTime LastActive { get; set; }
            public bool IsOnline { get; set; }
        }
    }
}
