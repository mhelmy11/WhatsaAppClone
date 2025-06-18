using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Service.Abstract
{
    public interface ITransactionService
    {
        public IDbContextTransaction BeginTransaction();

    }
}
