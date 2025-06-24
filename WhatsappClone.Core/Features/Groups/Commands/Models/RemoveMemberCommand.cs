using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.Models
{
    public class RemoveMemberCommand : IRequest<Response<string>>
    {
        public Guid GroupId { get; set; }
        public string UserId { get; set; }
    }

}
