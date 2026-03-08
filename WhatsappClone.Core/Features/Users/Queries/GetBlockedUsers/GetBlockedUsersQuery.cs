using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Users.Queries.GetBlockedUsers
{
    public record GetBlockedUsersQuery() : IRequest<Response<List<GetBlockedUsersResult>>>
    {
    }
}
