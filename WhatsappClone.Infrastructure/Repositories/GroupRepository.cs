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
    public class GroupRepository : SqlBaseRepository<Group>, IGroupRepository
    {
        private readonly SqlDBContext context;

        public GroupRepository(SqlDBContext context) : base(context)
        {
            this.context = context;
        }

        public bool IsAddMembersAllowed(Guid groupId)
        {
            return GetTableNoTracking()
                           .Any(g => g.Id == groupId && g.CanAddMembers);
        }

        public bool IsEditGroupAllowed(Guid groupId)
        {
            return GetTableNoTracking()
                           .Any(g => g.Id == groupId && g.CanEditGroupSettings);

        }

        public bool IsSendMessagesAllowed(Guid groupId)
        {
            return GetTableNoTracking()
                .Any(g => g.Id == groupId && g.CanSendMessages);
        }


    }
}
