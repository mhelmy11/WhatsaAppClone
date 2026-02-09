using System;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories;

public class MessageMentionRepository : MongoBaseRepository<MessageMention>, IMessageMentionRepository
{
    public MessageMentionRepository(IMongoDBFactory mongoDBFactory) : base(mongoDBFactory)
    {
    }

}
