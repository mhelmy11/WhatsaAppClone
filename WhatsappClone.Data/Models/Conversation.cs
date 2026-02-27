using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Data.Models
{

    [BsonCollection("conversations")]
    public class Conversation : MongoBaseModel
    {
      
        [BsonRepresentation(BsonType.Int64)]
        public long ChatId { get; set; }         

        [BsonElement("type")]
        public string Type { get; set; }          // "individual", "group"

        [BsonElement("participants")]
        [BsonRepresentation(BsonType.Int64)]
        public List<long> Participants { get; set; } = new(); // UserIds

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
