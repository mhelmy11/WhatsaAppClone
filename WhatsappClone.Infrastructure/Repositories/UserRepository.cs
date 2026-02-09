using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories
{
    public class UserRepository : SqlBaseRepository<AppUser>, IUserRepository
    {
        private readonly SqlDBContext dbContext;

        public UserRepository(SqlDBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }

}
