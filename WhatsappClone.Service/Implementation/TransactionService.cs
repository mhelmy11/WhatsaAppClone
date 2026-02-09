using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Infrastructure;

namespace WhatsappClone.Service.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly SqlDBContext _dbContext;

        public TransactionService(SqlDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IDbContextTransaction BeginTransaction()
        {


            return _dbContext.Database.BeginTransaction();
        }

    }
}
