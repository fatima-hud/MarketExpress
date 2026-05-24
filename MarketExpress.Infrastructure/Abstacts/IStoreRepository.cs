using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
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
        Task<bool> IsNameExistAsync(string name);
        Task<bool> IsNameExistAsync(string name,int storeId);
        Task<bool> IsUserExistByIdAndStatueApprovedAsync(string userId);
        Task<bool> IsUserHasPendingStoreAsync(string userId);
        Task<List<Store>> GetAllPendingStoreAsync();
        Task<List<Store>> GetAllActiveStoreAsync();
        Task<Store> GetStoreByAdminAsync(string ownerId);
        Task<List<Store>>GetAllRequestStoresByUserAsync(string userId);
        Task<List<Store>>GetAllRequestStoreByFilter(enStoreStatus storeStatus,int pageNumber,int pageSize);
        Task<List<Store>> GetFilterRequestStoresByUserAsync(string userId, enStoreStatus storeStatus);
     Task<bool>IsPhoneNumberExistAsync(string phoneNumber);
        Task<bool> IsEmailExistAsync(string email);
        Task<Wallet?> GetWalletByStoreIdAsync(int storeId);




    }
}
