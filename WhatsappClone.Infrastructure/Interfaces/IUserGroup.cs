using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces
{
    public interface IUserGroup : IRepo<UserGroup>
    {

        public bool IsUserInGroup(string userId, Guid groupId);
        public bool IsGroupAdmin(string userId, Guid groupId);


    }
}
