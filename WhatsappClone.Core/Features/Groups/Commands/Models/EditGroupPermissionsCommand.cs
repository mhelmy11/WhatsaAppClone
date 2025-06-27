using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.Models
{
    public class EditGroupPermissionsCommand : IRequest<Response<string>>
    {

        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool CanAddMembers { get; set; }

        public bool EditGroupSettings { get; set; }

        public bool AllowSendMessages { get; set; }

    }
}
