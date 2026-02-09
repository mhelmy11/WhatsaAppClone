using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Chats.Queries.Results
{
    public class GetChatsResponse
    {
        public int Id { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverProfilePic { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime LastMessageTime { get; set; }
        public int UnreadCount { get; set; }
        public bool isPinned { get; set; }

    }
}
