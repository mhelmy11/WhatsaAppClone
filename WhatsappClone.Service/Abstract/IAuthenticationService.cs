using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Service.Abstract
{
    public interface IAuthenticationService
    {

        public Task<JWTResult> GetToken(AppUser user);
    }
}
