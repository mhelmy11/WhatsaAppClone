using WhatsappClone.Data.Enums;
namespace WhatsappClone.Data.Helpers
{

    public class ChatDTO
    {
        public string? privateId { get; set; }//private chat
        public Guid? groupId { get; set; } // group
        public string senderId { get; set; }
        public string senderName { get; set; }

        public string chatName { get; set; } //chat name
        public bool isGroup { get; set; }
        public string chatPic { get; set; } //chat pic
        public object lastMessageContent { get; set; }
        public string messageStatus { get; set; } = MessageStatusString.Pending;
        public DateTime lastMessageTime { get; set; }
        public bool isSystemMessage { get; set; } = false;

        public int? ImageCount { get; set; }
        public int? VideoCount { get; set; }
        public int? AudioCount { get; set; }
        public bool isPinned { get; set; }
        public bool isMuted { get; set; }
    }
}
