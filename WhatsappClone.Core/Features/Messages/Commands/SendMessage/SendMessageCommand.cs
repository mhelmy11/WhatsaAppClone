using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Messages.Commands.SendMessage
{
    public class SendMessageCommand : IRequest<Response<SendMessageResult>>
    {
        public long? ChatId { get; set; }
        public long? RecipientId { get; set; }
        public string MessageType { get; set; }
        public string? RecipientType { get; set; }

        public MessageContentDto Content { get; set; }
        public List<RepliesDto>? Replies { get; set; }

        public List<long>? Mentions { get; set; }
        public string? ReplyToMessageId { get; set; }
        public long? ReplyToUserId { get; set; }

    }

    public class RepliesDto
    {
        public string MessageId { get; set; }
        public long UserId { get; set; }
    }

    public class MessageContentDto
    {
        // Text
        public string? Text { get; set; }
        public List<FormattedTextDto>? FormattedText { get; set; }
        public SystemEventDto? SystemEvent { get; set; }

        // Media
        public string? MediaUrl { get; set; }
        public string? MediaKey { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? FileName { get; set; }
        public int? FileSize { get; set; }
        public string? MimeType { get; set; }
        public int? Duration { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        // Location
        public LocationDto? Location { get; set; }

        // Contact
        public ContactInfoDto? Contact { get; set; }

        // Poll
        public PollDto? Poll { get; set; }


        // Sticker/GIF
        public string? StickerId { get; set; }
        public string? GifUrl { get; set; }

        // Caption
        public string? Caption { get; set; }

        // Link preview
        public string? Preview { get; set; }
    }

    public class SystemEventDto
    {
        public string EventType { get; set; }
        public long ActorId { get; set; }
        public List<long>? TargetIds { get; set; }
        public long? TargetGroupId { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
    public class FormattedTextDto
    {
        public string Type { get; set; } // "mention", "bold", "italic"
        public long? UserId { get; set; } // for mention
        public int Offset { get; set; }
        public int Length { get; set; }
    }

    public class LocationDto
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

    public class ContactInfoDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Vcard { get; set; }
    }

    public class PollDto
    {
        public string? Question { get; set; }
        public List<PollOptionDto> Options { get; set; } = new();
        public bool? MultipleChoice { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public class PollOptionDto
    {
        public string? Text { get; set; }
    }
     
}
