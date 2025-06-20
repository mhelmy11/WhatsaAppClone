using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Users.Commands.Results
{
    public class EditMeResult
    {
        public string? PicUrl { get; set; }
        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? About { get; set; }
    }
}
