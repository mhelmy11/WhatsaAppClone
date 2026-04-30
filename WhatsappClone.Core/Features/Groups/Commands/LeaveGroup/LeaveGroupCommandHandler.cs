using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Groups.Commands.RemoveGroupMember;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Groups.Commands.LeaveGroup
{
    public class LeaveGroupCommandHandler : ResponseHandler, IRequestHandler<LeaveGroupCommand, Response<LeaveGroupResult>>
    {
        private readonly CurrentUserService currentUserService;
        private readonly SqlDBContext dBContext;

        public LeaveGroupCommandHandler(CurrentUserService currentUserService , SqlDBContext dBContext)
        {
            this.currentUserService = currentUserService;
            this.dBContext = dBContext;
        }
        public async Task<Response<LeaveGroupResult>> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;

            var Member = await dBContext.GroupMembers
            .FirstOrDefaultAsync(gm => gm.GroupId == request.GroupId &&
                        (gm.UserId == currentUserId));


            if (Member == null)
            {
                return BadRequest<LeaveGroupResult>("this user is not in the group.");
            }


            if (Member.Role == MemberRole.Admin)
            {
                var hasOtherMembers = await dBContext.GroupMembers
                    .AnyAsync(gm => gm.GroupId == request.GroupId && gm.UserId != currentUserId, cancellationToken);

                if (hasOtherMembers)
                {
                    var otherAdminsExist = await dBContext.GroupMembers
                        .AnyAsync(gm => gm.GroupId == request.GroupId &&
                                        gm.UserId != currentUserId &&
                                        gm.Role == MemberRole.Admin, cancellationToken);

                    if (!otherAdminsExist)
                    {
                        return BadRequest<LeaveGroupResult>("You cannot leave the group because you are the only admin. Promote another member first.");
                    }
                }

            }

            dBContext.GroupMembers.Remove(Member);
            await dBContext.SaveChangesAsync(cancellationToken);

            //TODO....
            //trigger System Event message

            return Success<LeaveGroupResult>(new LeaveGroupResult(ActorUserId: currentUserId), "Left Successfully.");


        }
    }
}
