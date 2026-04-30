using Microsoft.EntityFrameworkCore;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Groups.Commands.JoinGroupViaInviteLink
{
    public class JoinGroupViaInviteLinkCommandHandler : ResponseHandler, IRequestHandler<JoinGroupViaInviteLinkCommand, Response<JoinGroupViaInviteLinkResult>>
    {
        private readonly CurrentUserService currentUserService;
        private readonly SqlDBContext dBContext;

        public JoinGroupViaInviteLinkCommandHandler(CurrentUserService currentUserService , SqlDBContext dBContext)
        {
            this.currentUserService = currentUserService;
            this.dBContext = dBContext;
        }
        public async Task<Response<JoinGroupViaInviteLinkResult>> Handle(JoinGroupViaInviteLinkCommand request, CancellationToken cancellationToken)
        {

            var currentUserId = currentUserService.UserId;


            //check InviteCode if exists or expired
            var InviteCode = request.InviteCode;

            var Group = await dBContext.Groups.FirstOrDefaultAsync(g=>g.InviteLink == InviteCode , cancellationToken);

            if (Group == null) 
            {
                return BadRequest<JoinGroupViaInviteLinkResult>("Invalid Invite Link ");
            }
            if (Group.InviteLinkExpiry < DateTime.UtcNow)
            {
                return BadRequest<JoinGroupViaInviteLinkResult>("Expired Invite Link");
            }
            var isAlreadyMember = await dBContext.GroupMembers
            .AnyAsync(gm => gm.GroupId == Group.Id && gm.UserId == currentUserId, cancellationToken);

            if (isAlreadyMember)
            {
                return BadRequest<JoinGroupViaInviteLinkResult>("User already joined");
            }

            var NewGroupMember = new GroupMember { GroupId = Group.Id, UserId = currentUserId };
             dBContext.GroupMembers.Add(NewGroupMember);
            await dBContext.SaveChangesAsync(cancellationToken);


            //TODO........
            //trigger joined via invite link event

            return Success<JoinGroupViaInviteLinkResult>(new JoinGroupViaInviteLinkResult(currentUserId, Group.Id));



        }
    }
}
