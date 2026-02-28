using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Core.Features.Identity.Commands
{
    public class RefreshTokenResult
    {

        public JWTResult tokens { get; set; }
    }
}
