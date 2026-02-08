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
    public class ToggleArchiveGroupCommand : IRequest<Response<string>>
    {



        [Required]
        public Guid groupId { get; set; }

        [Required]
        public bool currentState { get; set; }
    }
}
