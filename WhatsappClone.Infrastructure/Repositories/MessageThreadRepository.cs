using System;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories;

public class MessageThreadRepository : MongoBaseRepository<MessageThread>, IMessageThreadRepository
{
    public MessageThreadRepository(IMongoDBFactory mongoDBFactory) : base(mongoDBFactory)
    {
    }
}
