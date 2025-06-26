using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces
{
    public interface IGroup : IRepo<Group>
    {


        public bool IsSendMessagesAllowed(Guid groupId);
        public bool IsAddMembersAllowed(Guid groupId);
        public bool IsEditGroupAllowed(Guid groupId);


    }
}
