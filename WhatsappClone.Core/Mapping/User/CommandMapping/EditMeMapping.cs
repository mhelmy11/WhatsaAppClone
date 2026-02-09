using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Users.Commands.Models;
using WhatsappClone.Data.SqlServerModels;


namespace WhatsappClone.Core.Mapping.User
{
    public partial class UserProfile
    {

        public void EditMeMapping()
        {

            CreateMap<EditMeCommand, AppUser>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}