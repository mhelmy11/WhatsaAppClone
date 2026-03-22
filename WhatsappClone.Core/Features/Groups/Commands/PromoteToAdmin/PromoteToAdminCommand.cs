using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.PromoteToAdmin
{
    public record PromoteToAdminCommand(long GroupId , long PromotedUserId) : IRequest<Response<PromoteToAdminResult>>;
    
}
