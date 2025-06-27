using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Groups.Queries.Results;

namespace WhatsappClone.Core.Features.Groups.Queries.Models
{
    public class GetGroupInviteInfoQuery : IRequest<Response<GetGroupInviteInfoResult>>
    {

        [Required]
        public string inviteCode { get; set; } = null!;

        public GetGroupInviteInfoQuery(string inviteCode)
        {
            this.inviteCode = inviteCode;

        }
    }
}
