using AutoMapper.Configuration.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Users.Commands.Results;

namespace WhatsappClone.Core.Features.Users.Commands.Models
{
    public class EditMeCommand : IRequest<Response<EditMeResult>>
    {
        public string? FullName { get; set; }

        [Ignore] // This property will be ignored by AutoMapper
        public IFormFile? ProfilePic { get; set; }
        public string? About { get; set; }


    }
}
