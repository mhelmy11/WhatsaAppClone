using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.Models
{
    public class EditGroupPhotoCommand : IRequest<Response<string>>
    {
        public IFormFile? GroupProfilePic { get; set; }
        [Required]
        public Guid GroupId { get; set; }
    }
}
