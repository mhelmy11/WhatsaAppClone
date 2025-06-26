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
    public class EditGroupMessageCommand : IRequest<Response<string>>
    {

        public Guid groupId { get; set; }
        public Guid messageId { get; set; }


        [Required]
        public string content { get; set; }
    }
}
