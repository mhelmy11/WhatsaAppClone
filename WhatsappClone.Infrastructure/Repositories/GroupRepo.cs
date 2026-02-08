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
    public class GroupRepo : Repo<Group>, IGroup
    {
        private readonly Context context;

        public GroupRepo(Context context) : base(context)
        {
            this.context = context;
        }

        public bool IsAddMembersAllowed(Guid groupId)
        {
            return GetTableNoTracking()
                           .Any(g => g.GroupId == groupId && g.CanAddMembers);
        }

        public bool IsEditGroupAllowed(Guid groupId)
        {
            return GetTableNoTracking()
                           .Any(g => g.GroupId == groupId && g.EditGroupSettings);

        }

        public bool IsSendMessagesAllowed(Guid groupId)
        {
            return GetTableNoTracking()
                .Any(g => g.GroupId == groupId && g.AllowSendMessages);
        }


    }
}
