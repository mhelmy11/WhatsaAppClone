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
    public class MessageRepo : Repo<Message>, IMessage
    {
        private readonly Context context;

        public MessageRepo(Context context) : base(context)
        {
            this.context = context;
        }
    }
}
