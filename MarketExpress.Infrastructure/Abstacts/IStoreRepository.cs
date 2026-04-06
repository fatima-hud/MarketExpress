using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IStoreRepository:IGenericRepositoryAsync<Store>
    {
        Task<bool> IsNameFoundAsync(string name);
        Task<bool> IsUserExistByIdAndStatueApprovedAsync(string userId);
        Task<bool> IsUserHasPendingStoreAsync(string userId);
        Task<List<Store>> GetAllPendingStoreAsync();
        Task<List<Store>> GetAllActiveStoreAsync();
        Task<Store> GetStoreByAdminAsync(string ownerId);




    }
}
