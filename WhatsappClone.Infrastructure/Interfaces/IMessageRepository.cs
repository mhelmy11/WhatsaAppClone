using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces;
public interface IMessageRepository : IMongoBaseRepository<Message>
{

}


