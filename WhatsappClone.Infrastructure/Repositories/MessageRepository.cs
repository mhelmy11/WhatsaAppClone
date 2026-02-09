using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories
{
    public class MessageRepository : MongoBaseRepository<Message>, IMessageRepository
    {

        public MessageRepository(IMongoDBFactory mongoDBFactory) : base(mongoDBFactory)
        {

        }
      





    }
}
