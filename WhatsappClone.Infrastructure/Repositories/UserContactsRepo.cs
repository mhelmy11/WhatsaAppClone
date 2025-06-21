using Microsoft.EntityFrameworkCore;
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
    public class UserContactsRepo : Repo<UserContact>, IUserContacts
    {
        private readonly Context dbContext;

        public UserContactsRepo(Context dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<UserContact> GetContactsOrderedByAlpha(string userId)
        {
            return GetTableNoTracking()
                    .Where(uc => uc.UserId == userId)
                    .Include(uc => uc.Contact)
                    .OrderBy(uc => uc.FullName)
                    .ToList();
        }
    }
}
