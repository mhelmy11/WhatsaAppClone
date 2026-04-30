using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Groups.Commands.PromoteToAdmin;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Groups.Commands.RemoveGroupMember
{
    public class RemoveGroupMemberCommandHandler : ResponseHandler, IRequestHandler<RemoveGroupMemberCommand, Response<RemoveGroupMemberResult>>
    {
        private readonly CurrentUserService currentUserService;
        private readonly SqlDBContext dBContext;

        public RemoveGroupMemberCommandHandler(CurrentUserService currentUserService , SqlDBContext dBContext)
        {
            this.currentUserService = currentUserService;
            this.dBContext = dBContext;
        }
        public async Task<Response<RemoveGroupMemberResult>> Handle(RemoveGroupMemberCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;

            var groupMembers = await dBContext.GroupMembers
            .Where(gm => gm.GroupId == request.GroupId &&
                        (gm.UserId == currentUserId || gm.UserId == request.TargetUserId))
            .ToListAsync(cancellationToken);

            var actor = groupMembers.FirstOrDefault(gm => gm.UserId == currentUserId);
            if (actor == null || actor.Role != MemberRole.Admin)
            {
                return BadRequest<RemoveGroupMemberResult>("Unauthorized to remove this user. you should be admin.");
            
            }

            var targetMember = groupMembers.FirstOrDefault(gm => gm.UserId == request.TargetUserId);
            if (targetMember == null)
            {
                return BadRequest<RemoveGroupMemberResult>("this user is not in the group.");
            }

            if (currentUserId == request.TargetUserId)
            {
                var otherAdminsExist = await dBContext.GroupMembers
                    .AnyAsync(gm => gm.GroupId == request.GroupId && gm.UserId != currentUserId && gm.Role == MemberRole.Admin, cancellationToken);

                if (!otherAdminsExist)
                {
                    return BadRequest<RemoveGroupMemberResult>("You cannot leave the group because you are the only admin. Promote another member first.");
                }
            }

            dBContext.GroupMembers.Remove(targetMember);
            await dBContext.SaveChangesAsync(cancellationToken);

            //TODO....
            //trigger System Event message

            return Success<RemoveGroupMemberResult>(new RemoveGroupMemberResult(RemovedUserId: request.TargetUserId), "Removed Successfully.");


        }
    }
}
