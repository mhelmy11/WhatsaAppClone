using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Enums;
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

        public bool IsGroupAdmin(string userId, Guid groupId)
        {
            return GetTableNoTracking().Any(ug => ug.UserId == userId && ug.GroupId == groupId && ug.Role == GroupRoleString.Admin);
        }

        public bool IsUserInGroup(string userId, Guid groupId)
        {
            return GetTableNoTracking().Any(ug => ug.UserId == userId && ug.GroupId == groupId);
        }
    }
}
