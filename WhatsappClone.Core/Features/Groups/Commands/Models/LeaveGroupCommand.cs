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
    public class LeaveGroupCommand : IRequest<Response<string>>
    {

        [Required(ErrorMessage = "GroupId is required")]
        public Guid GroupId { get; set; }
    }
}
