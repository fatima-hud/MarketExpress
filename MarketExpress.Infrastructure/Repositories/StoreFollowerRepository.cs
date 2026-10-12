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

            var products = await _dbContext.Products.Include(p => p.Store)
                .Where(p => storeIds.Contains(p.StoreId))
                .ToListAsync();

            return products;

        }
     /*   public async Task<List<Product>> GetProductsofStoreFollowerByUserIdAsync(string userId) { 
            var products = await _dbContext.Products.Include(p => p.Store)
                .Where(p => _dbContext.StoreFollowers
                .Any(sf => sf.StoreId == p.StoreId && sf.UserId == userId && sf.IsFollow))
                .ToListAsync();
            return products; 
        }
     */
        public async  Task<List<Store>> GetStoreFollowByUserAsync(string userId)
        {
            var ret=await _dbContext.StoreFollowers.Where(sf=>sf.UserId == userId).Where(sf => sf.IsFollow).Select(sf => sf.Store).ToListAsync();
            return ret;

        }

        public async Task<StoreFollower?> GetStoreFollowerByUserAsync(string userId, int storeId)
        {
            var ret=await _dbContext.StoreFollowers.FirstOrDefaultAsync(e=>e.UserId==userId && e.StoreId==storeId);
            return ret;
        }

        public async  Task<int> GetStoreFollowersCountAsync(int storeId)
        {
            var ret=await _dbContext.StoreFollowers.Where(sf=>sf.StoreId == storeId && sf.IsFollow).CountAsync();
            return ret;
        }

      
    }
}
