using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Groups.Commands.PromoteToAdmin
{
    public class PromoteToAdminCommandHandler : ResponseHandler, IRequestHandler<PromoteToAdminCommand, Response<PromoteToAdminResult>>
    {
        private readonly SqlDBContext dBContext;
        private readonly ICurrentUserService currentUserService;

        public PromoteToAdminCommandHandler(SqlDBContext dBContext, ICurrentUserService currentUserService)
        {
            this.dBContext = dBContext;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<PromoteToAdminResult>> Handle(PromoteToAdminCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;

            var groupMembers = await dBContext.GroupMembers
            .Where(gm => gm.GroupId == request.GroupId &&
                        (gm.UserId == currentUserId || gm.UserId == request.PromotedUserId))
            .ToListAsync(cancellationToken);

            var actor = groupMembers.FirstOrDefault(gm => gm.UserId == currentUserId);
            if (actor == null || actor.Role != MemberRole.Admin)
            {
                return BadRequest<PromoteToAdminResult>("Unauthorized to promote this user. you should be admin");
            }

            var targetMember = groupMembers.FirstOrDefault(gm => gm.UserId == request.PromotedUserId);
            if (targetMember == null)
            {
                return BadRequest<PromoteToAdminResult>("this user is not in the group");
            }
            if (targetMember.Role == MemberRole.Admin)
            {
                return BadRequest<PromoteToAdminResult>("this user already admin");
            }

            targetMember.Role = MemberRole.Admin;

            await dBContext.SaveChangesAsync(cancellationToken);

            //trigger System Event message

                //var systemEventMessage = new Message
                //{
                //    ChatId = request.GroupId, // أو الـ ChatId المقابل للجروب لو مفصولين
                //    SenderId = currentUserId, // الـ Actor
                //    MessageType = MessageType.System,
                //    Timestamp = DateTime.UtcNow,
                //    Content = new MessageContent
                //    {
                //        SystemEvent = new SystemEvent
                //        {
                //            EventType = SystemEventType.PromoteToAdmin,
                //            ActorId = currentUserId,
                //            TargetIds = new List<long> { request.PromotedUserId },
                //            TargetGroupId = request.GroupId,
                //        }
                //    }
                //};

                //await _messagesCollection.InsertOneAsync(systemEventMessage, cancellationToken: cancellationToken);

            return Success<PromoteToAdminResult>(new PromoteToAdminResult(PromotedUserId: request.PromotedUserId), "Promoted Successfully");

        }
    }
}
