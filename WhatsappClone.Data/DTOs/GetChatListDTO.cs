using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.DTOs
{
    public class GetChatListDTO
    {
        public Guid ChatId { get; set; }
        public string? ReceiverId { get; set; }
        public Guid? GroupId { get; set; }
        public string ChatName { get; set; }
        public string ChatImage { get; set; }
        public string ChatType { get; set; }
        public bool IsPinned { get; set; }
        public bool IsMuted { get; set; }
        public MessageMetadataResponse? LastMessage { get; set; }
        public string LastReadMessageId { get; set; }
        public long UnreadMessagesCount { get; set; }
    }

    public class MessageMetadataResponse
    {
        public string MessageId { get; set; }
        public string? MessageStatus { get; set; }
        public bool IsLastMessageSentByMe { get; set; }
        public bool IsMentioningMe { get; set; }
        public string MessageContent { get; set; }
        public string MessageType { get; set; }
        public DateTime LastMessageTime { get; set; }
        public string? LastMessageSenderName { get; set; }
    }
}
