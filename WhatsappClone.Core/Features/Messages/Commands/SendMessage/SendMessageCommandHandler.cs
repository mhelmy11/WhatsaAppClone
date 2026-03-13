using IdGen;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Messages.Events;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Messages.Commands.SendMessage
{
    public class SendMessageCommandHandler : ResponseHandler, IRequestHandler<SendMessageCommand, Response<SendMessageResult>>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IMongoDBFactory mongoDBFactory;
        private readonly IIdGenerator<long> idGenerator;
        private readonly IMediator mediator;
        private readonly SqlDBContext dBContext;

        public SendMessageCommandHandler(
            ICurrentUserService currentUserService ,
            IMongoDBFactory mongoDBFactory ,
            IIdGenerator<long> idGenerator ,
            IMediator mediator ,
            SqlDBContext dBContext
            )
        {
            this.currentUserService = currentUserService;
            this.mongoDBFactory = mongoDBFactory;
            this.idGenerator = idGenerator;
            this.mediator = mediator;
            this.dBContext = dBContext;
        }
        public async Task<Response<SendMessageResult>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var messageCollection = mongoDBFactory.GetCollection<Message>();
            var conversationCollection = mongoDBFactory.GetCollection<Conversation>();
            long ChatId = 0;
            long PeerId = 0;

            string ConversationType = string.Empty;
            List<long> chatParticipants = new();
            List<long> extractedMentions = new();

            if (request.ChatId.HasValue && request.ChatId > 0) //old conversation 
            {
                ChatId = request.ChatId.Value;
                var filter = Builders<Conversation>.Filter.Eq(c => c.ChatId, request.ChatId);
                var exisitingconversation = await conversationCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
                if (exisitingconversation == null)
                {
                    return BadRequest<SendMessageResult>("Conversation not found");
                }
                ConversationType = exisitingconversation.Type;

                chatParticipants = exisitingconversation.Participants.ToList() ?? new();
                if(ConversationType == WhatsappClone.Data.Models.ConversationType.Group)
                {
                    //check if sender in the group
                    var isGroupMember = dBContext.GroupMembers.Any(gm=> gm.GroupId == ChatId && gm.UserId == currentUserId);

                    var isAdmin =
                        dBContext.GroupMembers.Any(gm=>gm.GroupId ==  ChatId && gm.UserId == currentUserId && gm.Role == MemberRole.Admin) ;

                    var isSendMessagesIsNotAllowed =
                        dBContext.Groups.Any(g => g.Id == ChatId && g.AllowMessagesForMembers == false);


                    if (!isGroupMember)
                    {
                        return BadRequest<SendMessageResult>("You are not allowed to send messages to this group");
                    }
                    if(!(isAdmin && isSendMessagesIsNotAllowed))
                    {
                        return BadRequest<SendMessageResult>("You are not allowed to send messages to this group");
                    }

                    if (request.Content.FormattedText is not null)
                    {
                        extractedMentions =  request.Content.FormattedText.Where(c => c.Type == "mention").Select(c => c.UserId.Value).Distinct().ToList();
                    }

                    PeerId = ChatId;
                }



            }
            else if (request.RecipientId.HasValue && request.RecipientId > 0)
            {
                var targetUserId = request.RecipientId.Value;
                ConversationType = WhatsappClone.Data.Models.ConversationType.Individual;
                PeerId = targetUserId;
                var existingChat = await conversationCollection.Find(c =>
                    c.Type == ConversationType &&
                    c.Participants.Contains(currentUserId) &&
                    c.Participants.Contains(targetUserId)
                ).FirstOrDefaultAsync(cancellationToken);

                if (existingChat != null)
                {
                    ChatId = existingChat.ChatId;
                    chatParticipants = existingChat.Participants;
                }
                else //new individual conversation
                {
                    ChatId = idGenerator.CreateId();


                    var newChat = new Conversation
                    {
                        ChatId = ChatId,
                        Type = WhatsappClone.Data.Models.ConversationType.Individual,
                        Participants = new List<long> { Math.Min(currentUserId, targetUserId), Math.Max(currentUserId, targetUserId) },
                        CreatedAt = DateTime.UtcNow,
                    };
                    ConversationType = newChat.Type;

                    await conversationCollection.InsertOneAsync(newChat, cancellationToken: cancellationToken);

                    chatParticipants = newChat.Participants;
                }

            }
            else 
            {
                return BadRequest<SendMessageResult>("Somethin went wrong");
            }

            var newMessage = new Message
            {
                ChatId = ChatId,
                SenderId = currentUserId,
                RecipientType = request.RecipientType,
                RecipientId = request.RecipientId,
                MessageType = request.MessageType,
                Timestamp = DateTime.UtcNow,
                Status = new MessageStatus
                {
                    SentAt = DateTime.UtcNow
                },
                ReplyMessageId = request.ReplyToMessageId,
                Mentions = request.Mentions,

                Content = new MessageContent
                {
                    Text = request.Content.Text,
                    MediaUrl = request.Content.MediaUrl,
                    FileName = request.Content.FileName,
                    FileSize = request.Content.FileSize,
                    MimeType = request.Content.MimeType,
                    Duration = request.Content.Duration,
                    FormattedText = request.Content.FormattedText is not null ? request.Content.FormattedText.Select(ft => new FormattedText
                    {
                        Length = ft.Length,
                        Offset = ft.Offset,
                        Type = ft.Type,
                        UserId = ft.UserId,
                    }).ToList() : null,
                    Contact = request.Content.Contact is not null ? new ContactInfo {
                        Name = request.Content.Contact.Name,
                        Phone = request.Content.Contact.Phone,
                        Vcard = request.Content.Contact.Vcard
                    } : null,

                    ThumbnailUrl = request.Content.ThumbnailUrl,
                    StickerId = request.Content.StickerId,
                    SystemEvent = request.Content.SystemEvent is not null ? new SystemEvent {
                        ActorId = currentUserId,
                        EventType = request.Content.SystemEvent.EventType,
                        NewValue = request.Content.SystemEvent.NewValue,
                        OldValue = request.Content.SystemEvent.OldValue,
                        TargetGroupId = request.Content.SystemEvent.TargetGroupId,
                        TargetIds = request.Content.SystemEvent.TargetIds
                    } : null,
                    Caption = request.Content.Caption,
                    GifUrl = request.Content.GifUrl,
                    Height = request.Content.Height,
                    MediaKey = request.Content.MediaKey,
                    Preview = request.Content.Preview,
                    Width = request.Content.Width,
                    Location = request.Content.Location is not null ? new Location
                    {
                        Address = request.Content.Location.Address,
                        Lat = request.Content.Location.Lat,
                        Lng = request.Content.Location.Lng,
                        Name = request.Content.Location.Name
                    } : null,
                    Poll = request.Content.Poll is not null ? new Poll
                    {
                        EndTime = request.Content.Poll.EndTime,
                        MultipleChoice = request.Content.Poll.MultipleChoice,
                        Question = request.Content.Poll.Question,
                        Options = request.Content.Poll.Options.Select(o => new PollOption
                        {
                            Text = o.Text,

                        }).ToList() ,

                    }: null
                    
                },
                Replies = request.Replies is not null && request.ReplyToMessageId is not null ? request.Replies.Select(r=> new Reply 
                { 
                    MessageId = request.ReplyToMessageId , UserId = request.ReplyToUserId 
                }).ToList()
                : null,
            };

            await messageCollection.InsertOneAsync(newMessage, cancellationToken: cancellationToken);
            string newId = newMessage.Id.ToString();

            //TODO...
            //Publish Event to Update UserConversation
            //it will be replaced with RabbitMQ ....
            await mediator.Publish(new MessageCreatedEvent(
                MessageId: newId,
                ChatId: ChatId,
                Caption: request.Content.Caption,
                ConversationType: ConversationType,
                SenderId: currentUserId,
                PeerId: PeerId,
                Participants: chatParticipants,
                MessageType: request.MessageType, // "image", "audio", "text"
                TextContent: request.Content?.Text,
                SentAt: newMessage.Timestamp,
                Mentions: extractedMentions
            ), cancellationToken);
            request.ChatId = ChatId;
            request.RecipientType = ConversationType;

            return Success(new SendMessageResult { MessageId = newId  , Message = request});
        }
    }
}
