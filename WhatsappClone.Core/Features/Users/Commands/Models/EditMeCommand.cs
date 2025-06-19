using AutoMapper.Configuration.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Users.Commands.Models
{
    public class EditMeCommand : IRequest<Response<string>>
    {
        public string? Id { get; set; }

        public string? UserName { get; set; }

        [Ignore]
        public IFormFile? ProfilePic { get; set; }
        public string? About { get; set; }


    }
}
