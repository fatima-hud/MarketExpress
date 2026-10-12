using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
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

       

        public async Task<bool> IsNameExistAsync(string name)
        {
          
            var res= await _dbContext.Stores.FirstOrDefaultAsync(e=>e.StoreName.ToLower()==name&& e.StoreStatus!=enStoreStatus.Rejected &&e.StoreStatus!=enStoreStatus.Cancelled &&e.StoreStatus!=enStoreStatus.Deleted);
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
            var stores = await _dbContext.Stores.Where(e => e.StoreStatus == enStoreStatus.Approved && e.IsActive).ToListAsync();
            return stores;
        }

        public async Task<Store> GetStoreByAdminAsync(string ownerId)
        {
            var store = await _dbContext.Stores.FirstOrDefaultAsync(e => e.OwnerId == ownerId&&e.IsActive);
            return store;
        }

        public async Task<Store> GetUnActiveStoreByAdminAsync(string ownerId)
        {
            var store = await _dbContext.Stores.FirstOrDefaultAsync(e => e.OwnerId == ownerId && e.StoreStatus==enStoreStatus.Approved);
            return store;
        }



        public async Task<bool> IsNameExistAsync(string name, int storeId)
        {
            var res = await _dbContext.Stores.FirstOrDefaultAsync(e => e.StoreName.ToLower() == name && e.StoreStatus != enStoreStatus.Rejected && e.StoreStatus != enStoreStatus.Cancelled && e.StoreStatus != enStoreStatus.Deleted && e.Id!=storeId);
            return res != null;
        }

        public async Task<List<Store>> GetAllRequestStoresByUserAsync(string userId)
        {
            var res = await _dbContext.Stores.IgnoreQueryFilters().Where(e => e.OwnerId == userId).ToListAsync();
            return res;
        }

        public async Task<List<Store>> GetAllRequestStoreByFilter(enStoreStatus storeStatus,int pageNumber,int pageSize)
        {
            var stors = await _dbContext.Stores.IgnoreQueryFilters().Where(e => e.StoreStatus == storeStatus).OrderByDescending(e=>e.CreatedAt).Skip(pageSize*(pageNumber-1)).Take(pageSize).ToListAsync();
            return stors;
        }

        public async Task<List<Store>> GetFilterRequestStoresByUserAsync(string userId,enStoreStatus storeStatus)
        {
            var res = await _dbContext.Stores.IgnoreQueryFilters().Where(e => e.OwnerId == userId && e.StoreStatus==storeStatus).ToListAsync();
            return res;
        }

        public async Task<bool> IsPhoneNumberExistAsync(string phoneNumber)
        {
            var res= await _dbContext.Stores.AnyAsync(e => e.PhoneNumber == phoneNumber && e.StoreStatus != enStoreStatus.Rejected && e.StoreStatus != enStoreStatus.Cancelled && e.StoreStatus != enStoreStatus.Deleted);
            return res;
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            var checkEmail=email.Trim().ToLower();
            var res=await _dbContext.Stores.AnyAsync(e => e.Email.ToLower() == checkEmail && e.StoreStatus != enStoreStatus.Rejected && e.StoreStatus != enStoreStatus.Cancelled && e.StoreStatus != enStoreStatus.Deleted);
            return res;
        }


        public async Task<Wallet?> GetWalletByStoreIdAsync(int storeId)
        {
            var store = await _dbContext.Stores.FirstOrDefaultAsync(e => e.Id == storeId);
           
            var res = await _dbContext.Wallets.FirstOrDefaultAsync(e => e.UserId == store.OwnerId);
            return res;
        }

     /*   public async Task<List<Store>> GetNearestStoreByUserIdAsync(decimal Latitude,decimal Longitude)
        {

              var stores = await _dbContext.Stores
        .Where(s => s.IsActive && !s.IsDeleted)
        .ToListAsync();

            return stores
                .OrderBy(s => GetDistance(
                    (double)Latitude,
                    (double)Longitude,
                    (double)s.Latitude,
                    (double)s.Longitude)).Take(10)
                .ToList();
        }
        private double GetDistance(
    double userLat,
    double userLon,
    double storeLat,
    double storeLon)
        {
            const double R = 6371; // نصف قطر الأرض بالكيلومتر

            var dLat = DegreesToRadians(storeLat - userLat);
            var dLon = DegreesToRadians(storeLon - userLon);

            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(userLat)) *
                Math.Cos(DegreesToRadians(storeLat)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }*/
    }
}
