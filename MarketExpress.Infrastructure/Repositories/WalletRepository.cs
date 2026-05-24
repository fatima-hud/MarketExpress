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

namespace MarketExpress.Infrastructure.Repositories
{
    public class WalletRepository : GenericRepositoryAsync<Wallet>, IWalletRepository
    {
        public WalletRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Wallet> GetByIdAsync(Guid id)
        {
            var res= await _dbContext.Wallets.FindAsync(id);
            return res;
          
        }

        public async Task<Wallet?> GetWalletByUserIdAsync(string userId)
        {
            var res = await _dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            return res;
        }


    }
}
