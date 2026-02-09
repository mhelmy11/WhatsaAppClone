using System;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces;

public interface IStarredMessageRepository : IMongoBaseRepository<StarredMessage>
{

}
