using System;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces;

public interface IUserChatSettingsRepository : ISqlBaseRepository<UserConversation>
{

}
