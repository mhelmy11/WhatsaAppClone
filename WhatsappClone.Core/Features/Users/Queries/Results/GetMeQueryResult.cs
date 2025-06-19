using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Users.Queries.Results
{
    public class GetMeQueryResult
    {
        public string? PicUrl { get; set; }
        public string? Username { get; set; }

        public string? PhoneNumber { get; set; }

        public string? About { get; set; }


    }
}
