using Base62;
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
        private readonly Base62Converter base62Converter;

        public CreateInviteLinkCommandHandler(SqlDBContext dBContext , Base62Converter base62Converter)
        {
            this.dBContext = dBContext;
            this.base62Converter = base62Converter;
        }
        public async Task<Response<CreateInviteLinkResult>> Handle(CreateInviteLinkCommand request, CancellationToken cancellationToken)
        {
            var group = await dBContext.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId , cancellationToken);
            if (group == null)
            {
                return BadRequest<CreateInviteLinkResult>("This group is not exisiting");
            }

            var decodedCode = $"{request.GroupId}+{DateTime.UtcNow}";

            var encodedCode = base62Converter.Encode(decodedCode);
            //add it to group table

            group.InviteLink = encodedCode;
            group.InviteLinkExpiry = DateTime.UtcNow.AddDays(7);
            await dBContext.SaveChangesAsync();

            return Success<CreateInviteLinkResult>(new CreateInviteLinkResult { InviteCode = encodedCode });

        }
    }
}
