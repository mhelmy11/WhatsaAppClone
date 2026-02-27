using System;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories;

public class UserChatSettingsRepository : SqlBaseRepository<UserConversation>, IUserChatSettingsRepository
{
    private readonly SqlDBContext sqlDBContext;

    public UserChatSettingsRepository(SqlDBContext sqlDBContext) : base(sqlDBContext)
    {
        this.sqlDBContext = sqlDBContext;
    }

}
