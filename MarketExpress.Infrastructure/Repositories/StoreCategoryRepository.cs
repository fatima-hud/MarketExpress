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
    public class StoreCategoryRepository : GenericRepositoryAsync<StoreCategory>, IStoreCategoryRepository
    {
        public StoreCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<bool> IsCategoryExistByStoreAsync(int storeId, int categoryId)
        {
            var res = await _dbContext.StoreCategories.AnyAsync(c => c.CategoryId == categoryId && c.StoreId == storeId);
            return res;
        }
        public async Task<List<StoreCategory>> GetAllStoreCategoryAsync(int storeId)
        {
            var res=await _dbContext.StoreCategories.AsNoTracking().Where(e=>e.StoreId==storeId ).ToListAsync();
            return res;
        }
    }
}
