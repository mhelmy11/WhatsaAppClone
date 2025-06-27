using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Groups.Queries.Results
{
    public class GetGroupInviteInfoResult
    {

        public string GroupName { get; set; } = null!;
        public string? GroupPic { get; set; }

        public int MembersCount { get; set; }


    }
}
