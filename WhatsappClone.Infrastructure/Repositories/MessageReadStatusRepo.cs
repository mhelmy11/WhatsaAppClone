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
    public class MessageReadStatusRepo : Repo<MessageReadStatus>,
        IMessageReadStatus
    {
        private readonly Context dbContext;

        public MessageReadStatusRepo(Context dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
