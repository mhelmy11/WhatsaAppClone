using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.UpdateGroupInfo
{
    public record UpdateGroupInfoCommand(
        long GroupId,
        string? GroupDescription,
        string? GroupPic,
        string? GroupName
        ) : IRequest<Response<UpdateGroupInfoResult>>;   
  
}
