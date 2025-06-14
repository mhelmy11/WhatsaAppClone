using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Helpers
{
    public class JWTResult
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }

    }
}
