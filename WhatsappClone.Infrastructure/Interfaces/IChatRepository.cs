using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Repositories;

namespace WhatsappClone.Infrastructure.Interfaces;

public interface IChatRepository : ISqlBaseRepository<Chat>
{
    public Task<List<Chat>> GetChatsAsync();

}
