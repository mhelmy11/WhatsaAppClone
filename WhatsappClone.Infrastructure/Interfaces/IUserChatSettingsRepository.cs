using System;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces;

public interface IUserChatSettingsRepository : ISqlBaseRepository<UserChatSettings>
{

}
