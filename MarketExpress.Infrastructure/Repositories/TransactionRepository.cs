using MarketExpress.Core.Entity;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MarketExpress.Infrastructure.Repositories
{
    public class TransactionRepository : GenericRepositoryAsync<TransactionProcess>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<TransactionProcess>> GetAllAsync(Guid walletId)
        {
       var res= await _dbContext.Transactions.Where(e=>e.WalletId==walletId).OrderByDescending(e=>e.Date).ToListAsync();
            return res;
        }
    }
}
