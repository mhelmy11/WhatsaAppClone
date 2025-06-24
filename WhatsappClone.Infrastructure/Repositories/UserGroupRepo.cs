using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories
{
    public class UserGroupRepo : Repo<UserGroup>, IUserGroup
    {
        private readonly Context dbContext;

        public UserGroupRepo(Context dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
