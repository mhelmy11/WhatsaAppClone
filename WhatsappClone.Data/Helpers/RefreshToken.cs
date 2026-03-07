using System.Text.Json.Serialization;

namespace WhatsappClone.Data.Helpers
{
    public class RefreshToken
    {


        [JsonIgnore]
        public int Id { get; set; }
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public string? PhoneNumber { get; set; }
    }
}