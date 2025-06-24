using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Core.Features.Groups.Queries.Models
{
    public class GetGroupListQuery : IRequest<Response<List<ChatDTO>>>
    {
    }
}
