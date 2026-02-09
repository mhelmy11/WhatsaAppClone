using System;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories;

public class StarredMessageRepository : MongoBaseRepository<StarredMessage>, IStarredMessageRepository
{
    public StarredMessageRepository(IMongoDBFactory mongoDBFactory) : base(mongoDBFactory)
    {
    }
}
