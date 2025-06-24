
using AutoMapper.Configuration.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Groups.Commands.Models
{
    public class CreateGroupCommand : IRequest<Response<Guid>>
    {
        public string Name { get; set; }

        public IFormFile? GroupPictureUrl { get; set; }


        [Ignore]
        public List<string> UserIDs { get; set; }

        // List of Group Settings....TODO..
    }
}
