using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Users.Commands.UpdateProfile
{
    public record UpdateProfileCommand(string? ProfilePic ,string? Name ,  string? About) : IRequest<Response<string>>
    {
    }
}
