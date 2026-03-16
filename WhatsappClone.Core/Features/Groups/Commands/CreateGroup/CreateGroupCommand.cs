
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.CreateGroup
{
    public record CreateGroupCommand(string Name , string? ProfilePic , GroupPermissions GroupPermissions  , List<long> Members) : IRequest<Response<CreateGroupResult>>
    {
    }

    public class GroupPermissions
    {

        public bool EditGroupSettings { get; set; } = true;
        public bool SendNewMessages { get; set; } = true;
        public bool SendNewAddMembers { get; set; } = true;
        public bool InviteViaLinke { get; set; } = false;
        public bool ApproveNewMembers { get; set; } = false;

    }
}
