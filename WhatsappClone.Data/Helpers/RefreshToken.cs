namespace WhatsappClone.Data.Helpers
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public string PhoneNumber { get; set; }
    }
}