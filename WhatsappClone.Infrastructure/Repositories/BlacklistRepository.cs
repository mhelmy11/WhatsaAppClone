using System;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories;

public class BlacklistRepository : SqlBaseRepository<Blacklist>, IBlacklistRepository
{
    private readonly SqlDBContext sqlDBContext;

    public BlacklistRepository(SqlDBContext sqlDBContext) : base(sqlDBContext)
    {
        this.sqlDBContext = sqlDBContext;
    }
}
