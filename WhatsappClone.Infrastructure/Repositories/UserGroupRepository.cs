using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories
{
    public class UserGroupRepository : SqlBaseRepository<UserGroup>, IUserGroupRepository
    {
        private readonly SqlDBContext dbContext;

        public UserGroupRepository(SqlDBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}
