using WhatsappClone.Data.Enums;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Data.Helpers
{

    public class ChatDTO
    {
        public Guid Id { get; set; } // group or private
        public string Name { get; set; } //chat name
        public bool isGroup { get; set; }
        public string PicUrl { get; set; } //chat pic
        public object LastMessageContent { get; set; }
        public List<MessageStatus>? MessageStatus { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public bool isSystemMessage { get; set; } = false;
        public List<AttachmentDTO>? Attachments { get; set; }

        public int? ImageCount { get; set; }

        public int? VideoCount { get; set; }
        public bool isPinned { get; set; }
        public bool isMuted { get; set; }
    }
}
