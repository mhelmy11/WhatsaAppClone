using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.UpdateGroupSettings
{
    public record UpdateGroupSettingsCommand(long GroupId , bool? EditGroupSettings , bool? SendNewMessages , bool? AddOtherMembers) : IRequest<Response<UpdateGroupSettingsResult>>;
}
