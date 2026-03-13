using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Messages.Queries.GetChatMessagesQuery
{
    public record GetChatMessagesResult(
     long MessageId,      
     long SenderId,
     string? SenderName,
     string? ProfilePic,
     string? TagName,
     DateTime SentAt,        
     string MessageType,     
     string Content,
     string? MediaUrl,
     string? MimeType ,
     string? FileSize,
     int? Duration , 
     string? ThumbnailUrl,   
     string? Caption,
     string? StickerId,
     string? GifUrl,
     string? Preview,         // Link Previews (OpenGraph)
     string Status,           // (Sent, Delivered, Read)
     RepliedMessagePreview? RepliedTo
 );

    public record RepliedMessagePreview(
        long OriginalMessageId,
        string OriginalSenderName,
        string Snippet,          //first 50 char from original message
        string? ThumbnailUrl   //for media | sticker  | Gif
    );
}
