using IdGen;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Groups.Events;
using WhatsappClone.Core.Features.Messages.Events;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandHandler : ResponseHandler, IRequestHandler<CreateGroupCommand, Response<CreateGroupResult>>
    {
        private readonly SqlDBContext dBContext;
        private readonly ICurrentUserService currentUserService;
        private readonly IIdGenerator<long> idGenerator;
        private readonly IMediator mediator;
        private readonly IMongoDBFactory mongoDBFactory;

        public CreateGroupCommandHandler(
            SqlDBContext dBContext,
            ICurrentUserService currentUserService ,
            IIdGenerator<long> idGenerator , 
            IMediator mediator , 
            IMongoDBFactory mongoDBFactory

            )
        {
            this.dBContext = dBContext;
            this.currentUserService = currentUserService;
            this.idGenerator = idGenerator;
            this.mediator = mediator;
            this.mongoDBFactory = mongoDBFactory;
        }
        public async Task<Response<CreateGroupResult>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var actortUserId = currentUserService.UserId;
            if (!request.Members.Any())
            {
                return BadRequest<CreateGroupResult>("At least 1 Member");
            }
            var MemberIncludingCreator = new List<long>(request.Members);
            MemberIncludingCreator.Add(actortUserId);
            
            //create new group and add members and mark the actor as admin

            var newGroupId = idGenerator.CreateId();
            var newGroup = new Group {
                Id = newGroupId ,
                CreatedBy = actortUserId ,
                Name = request.Name ,
                AllowMessagesForMembers = request.GroupPermissions.SendNewAddMembers,
                ApprovalRequiredToJoin = request.GroupPermissions.ApproveNewMembers,
                EditGroupDescription = request.GroupPermissions.EditGroupSettings,
                ProfilePicUrl = request.ProfilePic?? "default_group_avatar",
                Members = MemberIncludingCreator.Select(c=> new GroupMember
                {
                    GroupId = newGroupId,
                    UserId = c,
                    Role = c==actortUserId? MemberRole.Admin : MemberRole.Member,
                    JoinedAt = DateTime.UtcNow,
                    Status = MemberStatus.Active


                }).ToList()
            };

            await dBContext.AddAsync(newGroup);
            await dBContext.SaveChangesAsync();

            var conversationCollection = mongoDBFactory.GetCollection<Conversation>();

            //add new Conversation..........
            var newChat = new Conversation
            {
                ChatId = newGroupId,
                Type = WhatsappClone.Data.Models.ConversationType.Group,
                Participants = MemberIncludingCreator,
                CreatedAt = newGroup.CreatedAt,
            };

            await conversationCollection.InsertOneAsync(newChat, cancellationToken: cancellationToken);


            //Fire MessageCreatedEvent (System Message (Add to group)
            var newSystemMessage = new Message
            {
                ChatId = newGroupId,
                Content = new MessageContent
                {
                    SystemEvent = new SystemEvent
                    {
                        ActorId = actortUserId,
                        EventType = SystemEventType.AddMember,
                        TargetGroupId = newGroupId,
                        TargetIds = request.Members
                    }
                },
                SenderId = actortUserId ,
                RecipientType = ConversationType.Group,
                MessageType = MessageType.System,
            };

            request.Members.Add(actortUserId);
            var messagesCollection = mongoDBFactory.GetCollection<Message>();


            await messagesCollection.InsertOneAsync(newSystemMessage, cancellationToken: cancellationToken);
            string newId = newSystemMessage.Id.ToString();
            //TODO...
            //Publish Event to Update UserConversation
            //it will be replaced with RabbitMQ ....
            await mediator.Publish(new MessageCreatedEvent(
                  MessageId: newId,
                 ChatId: newGroupId,
                Caption: null,
                ConversationType: ConversationType.Group,
                SenderId: actortUserId,
                PeerId: null,
                Participants: request.Members,
                MessageType: MessageType.System,
                TextContent:null,
                SentAt: newGroup.CreatedAt,
                Mentions: null,
                MediaCount: 0,
                IsNewChat: false
                ), cancellationToken);


            return Success(new CreateGroupResult { GroupId = newGroupId });


        }
    }
}
