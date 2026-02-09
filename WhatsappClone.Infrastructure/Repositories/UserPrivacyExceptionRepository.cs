using System;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories;

public class UserPrivacyExceptionRepository : SqlBaseRepository<UserPrivacyException>, IUserPrivacyExceptionRepository
{
    public UserPrivacyExceptionRepository(SqlDBContext sqlDBContext) : base(sqlDBContext)
    {
    }
}
