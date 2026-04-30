using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Groups.Commands.JoinGroupViaInviteLink;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Groups.Queries.GetGroupInfoViaInviteLink
{
    public class GetGroupInfoViaInviteLinkQueryHandler : ResponseHandler , IRequestHandler<GetGroupInfoViaInviteLinkQuery, Response<GetGroupInfoViaInviteLinkResult>>
    {
        private readonly CurrentUserService currentUserService;
        private readonly SqlDBContext dBContext;

        public GetGroupInfoViaInviteLinkQueryHandler(CurrentUserService currentUserService  , SqlDBContext dBContext)
        {
            this.currentUserService = currentUserService;
            this.dBContext = dBContext;
        }
        public async Task<Response<GetGroupInfoViaInviteLinkResult>> Handle(GetGroupInfoViaInviteLinkQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var InviteCode = request.InviteCode;

            var groupInfo = await dBContext.Groups
               .AsNoTracking() 
               .Where(g => g.InviteLink == request.InviteCode)
               .Select(g => new
               {
                   g.Id,
                   g.Name,
                   g.ProfilePicUrl,
                   g.CreatedAt,
                   g.CreatedBy,
                   g.InviteLinkExpiry,
                   MemberCount = dBContext.GroupMembers.Count(gm => gm.GroupId == g.Id),
                   IsAlreadyMember = dBContext.GroupMembers.Any(gm => gm.GroupId == g.Id && gm.UserId == currentUserId)
               })
               .FirstOrDefaultAsync(cancellationToken);

            if (groupInfo == null)
            {
                return BadRequest<GetGroupInfoViaInviteLinkResult>("Invalid Invite Link ");
            }
            if (groupInfo.InviteLinkExpiry < DateTime.UtcNow)
            {
                return BadRequest<GetGroupInfoViaInviteLinkResult>("Expired Invite Link");
            }
        


            return Success<GetGroupInfoViaInviteLinkResult>(new GetGroupInfoViaInviteLinkResult(
                GroupName: groupInfo.Name,
                GroupPic: groupInfo.ProfilePicUrl,
                MembresCount: groupInfo.MemberCount,
                CreationDate: groupInfo.CreatedAt,
                CreatorUserId: groupInfo.CreatedBy,
                IsAlreadyMember: groupInfo.IsAlreadyMember
                ));


        }
    }
}
