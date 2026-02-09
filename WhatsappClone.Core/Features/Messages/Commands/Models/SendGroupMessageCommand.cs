using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.SqlServerModels;

namespace WhatsappClone.Core.Features.Messages.Commands.Models
{
    public class SendGroupMessageCommand : IRequest<Response<string>>
    {

        public Guid GroupId { get; set; }

        public string? Content { get; set; }

        public List<IFormFile>? Attachments { get; set; }
    }
}
