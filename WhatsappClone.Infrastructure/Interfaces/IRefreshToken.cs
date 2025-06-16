using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces
{
    public interface IRefreshToken : IRepo<TokenRefreshing>
    {
        public TokenRefreshing GetRefreshToken(string token);

        public void RevokeRefreshToken(string token);
    }
}
