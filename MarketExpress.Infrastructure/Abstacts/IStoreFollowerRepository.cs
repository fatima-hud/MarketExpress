using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IStoreFollowerRepository:IGenericRepositoryAsync<StoreFollower>
    {
        Task<StoreFollower?> GetStoreFollowerByUserAsync(string userId,int storeId);
        Task<List<Product>> GetProductsofStoreFollowerByUserIdAsync(string userId);
        Task<int>GetStoreFollowersCountAsync(int storeId);
        Task<List<Store>> GetStoreFollowByUserAsync(string userId);
    }
}
