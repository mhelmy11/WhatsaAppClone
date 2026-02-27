using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.Models
{
    [BsonCollection("statuses")]
    public class Status : MongoBaseModel
    {
        public string Id { get; set; }

        [BsonElement("user_id")]
        public string UserId { get; set; }

        [BsonElement("media_type")]
        public string MediaType { get; set; }

        [BsonElement("media_url")]
        public string MediaUrl { get; set; }

        [BsonElement("media_key")]
        public string MediaKey { get; set; }

        [BsonElement("thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("background_color")]
        public string BackgroundColor { get; set; }

        [BsonElement("font_style")]
        public string FontStyle { get; set; }

        [BsonElement("link_url")]
        public string LinkUrl { get; set; }

        [BsonElement("link_title")]
        public string LinkTitle { get; set; }

        [BsonElement("link_description")]
        public string LinkDescription { get; set; }

        [BsonElement("link_image")]
        public string LinkImage { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [BsonElement("expire_at")]
        public DateTime ExpireAt { get; set; } = DateTime.UtcNow.AddHours(24);

        [BsonElement("privacy_type")]
        public string PrivacyType { get; set; } = "everyone";

        [BsonElement("privacy_exceptions")]
        public List<string> PrivacyExceptions { get; set; } = new();

        [BsonElement("privacy_allowed")]
        public List<string> PrivacyAllowed { get; set; } = new();

        [BsonElement("viewers")]
        public List<StatusViewer> Viewers { get; set; } = new();

        [BsonElement("view_count")]
        public int ViewCount { get; set; }

        [BsonElement("screenshot_taken")]
        public List<string> ScreenshotTaken { get; set; } = new();

        [BsonElement("replies")]
        public List<StatusReply> Replies { get; set; } = new();
    }

    public class StatusViewer
    {
        public string UserId { get; set; }
        public DateTime ViewedAt { get; set; }
        public int Duration { get; set; } // watch time in seconds
    }

    public class StatusReply
    {
        public string UserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
