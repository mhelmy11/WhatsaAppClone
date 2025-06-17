using AutoMapper.Configuration.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Users.Commands.Models
{
    public class AddUserCommand : IRequest<Response<string>>
    {

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        [Ignore]
        public IFormFile? ProfilePic { get; set; }
        public string? PicUrl { get; set; }
    }
}
