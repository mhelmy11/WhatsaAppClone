using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    }
}
