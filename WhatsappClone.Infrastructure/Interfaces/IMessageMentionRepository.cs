using System;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces;

public interface IMessageMentionRepository : IMongoBaseRepository<MessageMention>
{

}
