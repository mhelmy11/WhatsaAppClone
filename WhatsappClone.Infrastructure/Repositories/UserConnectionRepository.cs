using System;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories;

public class UserConnectionRepository : SqlBaseRepository<UserConnection>, IUserConnectionRepository
{
    private readonly SqlDBContext sqlDBContext;

    public UserConnectionRepository(SqlDBContext sqlDBContext) : base(sqlDBContext)
    {
        this.sqlDBContext = sqlDBContext;
    }
}
