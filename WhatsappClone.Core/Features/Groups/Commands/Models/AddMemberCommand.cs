using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.Models
{
    public class AddMemberCommand : IRequest<Response<string>>
    {

        public string userId { get; set; }

        public Guid groupId { get; set; }
    }
}
