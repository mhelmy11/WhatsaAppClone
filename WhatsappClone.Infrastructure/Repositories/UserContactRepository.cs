using Microsoft.EntityFrameworkCore;
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
    public class UserContactRepository : SqlBaseRepository<UserContact>, IUserContactRepository
    {
        private readonly SqlDBContext dbContext;

        public UserContactRepository(SqlDBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
