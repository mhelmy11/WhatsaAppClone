using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Users.Queries.Results;

namespace WhatsappClone.Core.Features.Users.Queries.Models
{
    public class GetMeQuery : IRequest<Response<GetMeQueryResult>>
    {

    }
}
