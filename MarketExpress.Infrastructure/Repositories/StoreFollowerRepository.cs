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
    public class StoreFollowerRepository : GenericRepositoryAsync<StoreFollower>, IStoreFollowerRepository
    {
        public StoreFollowerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Product>> GetProductsofStoreFollowerByUserIdAsync(string userId)
        {
            var storeIds = await _dbContext.StoreFollowers
       .Where(e => e.UserId == userId && e.IsFollow)
       .Select(e => e.StoreId)
       .ToListAsync();

            if (!storeIds.Any())
                return new List<Product>();

            var products = await _dbContext.Products
                .Where(p => storeIds.Contains(p.StoreId))
                .ToListAsync();

            return products;

        }

        public async Task<StoreFollower?> GetStoreFollowerByUserAsync(string userId, int storeId)
        {
            var ret=await _dbContext.StoreFollowers.FirstOrDefaultAsync(e=>e.UserId==userId && e.StoreId==storeId);
            return ret;
        }
    }
}
