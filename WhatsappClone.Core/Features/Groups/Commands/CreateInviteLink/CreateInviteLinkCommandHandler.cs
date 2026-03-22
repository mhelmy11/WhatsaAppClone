using Base62;
using HashidsNet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Infrastructure;

namespace WhatsappClone.Core.Features.Groups.Commands.CreateInviteLink
{
    public class CreateInviteLinkCommandHandler : ResponseHandler, IRequestHandler<CreateInviteLinkCommand, Response<CreateInviteLinkResult>>
    {
        private readonly SqlDBContext dBContext;
        private readonly IHashids _hashids;

        public CreateInviteLinkCommandHandler(SqlDBContext dBContext , IHashids hashids)
        {
            this.dBContext = dBContext;
            _hashids = hashids;
        }
        public async Task<Response<CreateInviteLinkResult>> Handle(CreateInviteLinkCommand request, CancellationToken cancellationToken)
        {
            var group = await dBContext.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId , cancellationToken);
            if (group == null)
            {
                return BadRequest<CreateInviteLinkResult>("This group is not exisiting");
            }


            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var hash = _hashids.EncodeLong(request.GroupId , currentTimestamp);


            //add it to group table
            group.InviteLink = hash;
            group.InviteLinkExpiry = DateTime.UtcNow.AddDays(7);
            await dBContext.SaveChangesAsync();

            return Success<CreateInviteLinkResult>(new CreateInviteLinkResult { InviteCode = hash });

        }
    }
}
