using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Core.Features.Identity.Commands
{
    public class VerifyOtpResult
    {
        public bool IsNewUser { get; set; }

        public string? Name { get; set; }

        public string? ProfilePic { get; set; }
        public JWTResult tokens {  get; set; }
    }
}
