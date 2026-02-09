using System;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces;

public interface IMessageThreadRepository : IMongoBaseRepository<MessageThread>
{

}
