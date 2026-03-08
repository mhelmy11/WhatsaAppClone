using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Users.Queries.GetBlockedUsers
{
    public class GetBlockedUsersResult
    {

        public string Name { get; set; }
        public string ProfilePic { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public long BlockedUserId { get; set; }

    }
}
