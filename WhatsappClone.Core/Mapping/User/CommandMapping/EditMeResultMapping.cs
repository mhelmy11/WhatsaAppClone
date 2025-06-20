using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Users.Commands.Models;
using WhatsappClone.Core.Features.Users.Commands.Results;
using WhatsappClone.Data.Models;


namespace WhatsappClone.Core.Mapping.User
{
    public partial class UserProfile
    {

        public void EditMeResultMapping()
        {

            CreateMap<AppUser, EditMeResult>();

        }
    }
}