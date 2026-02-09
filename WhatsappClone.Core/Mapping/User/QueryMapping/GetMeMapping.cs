using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Users.Queries.Results;
using WhatsappClone.Data.SqlServerModels;
namespace WhatsappClone.Core.Mapping.User
{
    public partial class UserProfile
    {

        public void GetMeMapping()
        {

            CreateMap<AppUser, GetMeQueryResult>();
        }
    }

}