using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class StoreRepository : GenericRepositoryAsync<Store>, IStoreRepository
    {
        public StoreRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

       

        public async Task<bool> IsNameFoundAsync(string name)
        {
          
            var res= await _dbContext.Stores.FirstOrDefaultAsync(e=>e.StoreName.ToLower()==name.Trim().ToLower() && e.StoreStatus!=enStoreStatus.Rejected);
            return res != null;
        }

        public async Task<bool> IsUserExistByIdAndStatueApprovedAsync(string Id)
        {
            var res=await _dbContext.Stores.FirstOrDefaultAsync(e=>e.OwnerId==Id && e.StoreStatus==enStoreStatus.Approved);
            return res != null;
        }

        public async Task<bool> IsUserHasPendingStoreAsync(string userId)
        {
            var res = await _dbContext.Stores.FirstOrDefaultAsync(e => e.OwnerId == userId && e.StoreStatus == enStoreStatus.Pending);
            return res != null;
        }
        public async Task<List<Store>> GetAllPendingStoreAsync()
        {
            var stors = await _dbContext.Stores.Where(e => e.StoreStatus == enStoreStatus.Pending && !e.IsActive).ToListAsync();
            return stors;
        }

        public async Task<List<Store>> GetAllActiveStoreAsync()
        {
            var stores = await _dbContext.Stores.Include(e=>e.Owner).Where(e => e.StoreStatus == enStoreStatus.Approved && e.IsActive).ToListAsync();
            return stores;
        }

        public async Task<Store> GetStoreByAdminAsync(string ownerId)
        {
            var store = await _dbContext.Stores.FirstOrDefaultAsync(e => e.OwnerId == ownerId);
            return store;
        }
    }
}
