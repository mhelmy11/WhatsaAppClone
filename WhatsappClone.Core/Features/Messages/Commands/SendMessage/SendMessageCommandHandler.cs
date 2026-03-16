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
            ICurrentUserService currentUserService,
            IMongoDBFactory mongoDBFactory,
            IIdGenerator<long> idGenerator,
            IMediator mediator,
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
            long? PeerId;
            bool isNewChat = false;
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
                if (ConversationType == WhatsappClone.Data.Models.ConversationType.Group)
                {
                    //check if sender in the group
                    var isGroupMember = dBContext.GroupMembers.Any(gm => gm.GroupId == ChatId && gm.UserId == currentUserId);

                    var isAdmin =
                        dBContext.GroupMembers.Any(gm => gm.GroupId == ChatId && gm.UserId == currentUserId && gm.Role == MemberRole.Admin);

                    var isSendMessagesIsNotAllowed =
                        dBContext.Groups.Any(g => g.Id == ChatId && g.AllowMessagesForMembers == false);


                    if (!isGroupMember)
                    {
                        return BadRequest<SendMessageResult>("You are not allowed to send messages to this group");
                    }
                    if (isSendMessagesIsNotAllowed && !isAdmin)
                    {
                        return BadRequest<SendMessageResult>("You are not allowed to send messages to this group");
                    }

                    if (request.Content.FormattedText is not null)
                    {
                        extractedMentions = request.Content.FormattedText.Where(c => c.Type == "mention").Select(c => c.UserId.Value).Distinct().ToList();
                    }

                    PeerId = null;
                }
                else // existing Individual Conversation
                {
                    PeerId = request.RecipientId;
                }


            }
            else if (request.RecipientId.HasValue && request.RecipientId > 0) // so ChatId == null -> new individual Conversaion
            {
                ConversationType = WhatsappClone.Data.Models.ConversationType.Individual;
                PeerId = request.RecipientId.Value;
                ChatId = idGenerator.CreateId();
                var newChat = new Conversation
                {
                    ChatId = ChatId,
                    Type = ConversationType,
                    Participants = new List<long> { Math.Min(currentUserId, PeerId.Value), Math.Max(currentUserId, PeerId.Value) },
                    CreatedAt = DateTime.UtcNow,
                };
                await conversationCollection.InsertOneAsync(newChat, cancellationToken: cancellationToken);
                isNewChat = true;
                chatParticipants = newChat.Participants;
            }
            else
            {
                return BadRequest<SendMessageResult>("Somethin went wrong");
            }

            var newMessage = new Message
            {
                ChatId = ChatId,
                SenderId = currentUserId,
                RecipientType = ConversationType,
                RecipientId = PeerId,
                MessageType = request.MessageType,
                Timestamp = DateTime.UtcNow,
                Status = new MessageStatus
                {
                    SentAt = DateTime.UtcNow,
                },
                ReplyMessageId = request.ReplyToMessageId,

                Content = new MessageContent
                {
                    Text = request.Content.Text,
                    Caption = request.Content.Caption,
                    Media = request.Content.MediaDto is not null ? request.Content.MediaDto.Select(md => new MediaMessage
                    {
                        MediaUrl = md.MediaUrl,
                        FileName = md.FileName,
                        FileSize = md.FileSize,
                        MimeType = md.MimeType,
                        Duration = md.Duration,
                        ThumbnailUrl = md.ThumbnailUrl,
                        StickerId = md.StickerId,
                        GifUrl = md.GifUrl,
                        Height = md.Height,
                        Preview = md.Preview,
                        Width = md.Width

                    }).ToList() : null,

                    FormattedText = request.Content.FormattedText is not null ? request.Content.FormattedText.Select(ft => new FormattedText
                    {
                        Length = ft.Length,
                        Offset = ft.Offset,
                        Type = ft.Type,
                        UserId = ft.UserId,
                    }).ToList() : null,
                    Contact = request.Content.Contact is not null ? new ContactInfo
                    {
                        Name = request.Content.Contact.Name,
                        Phone = request.Content.Contact.Phone,
                    } : null,

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

                        }).ToList(),

                    } : null

                },

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
                Mentions: extractedMentions,
                MediaCount: request.Content.MediaCount,
                IsNewChat: isNewChat
            ), cancellationToken);
            request.ChatId = ChatId;
            request.RecipientType = ConversationType;
            request.RecipientId = PeerId;

            return Success(new SendMessageResult { MessageId = newId, Message = request });
        }
    }
}
