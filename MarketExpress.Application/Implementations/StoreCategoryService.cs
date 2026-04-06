using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
   /* public class StoreCategoryService : IStoreCategoryService
    {
     
        private readonly IUserRoleStoreRepository _userRoleStoreRepository;

        public StoreCategoryService(IStoreCategoryRepository repository, IUserRoleStoreRepository userRoleStoreRepository)
        {
            _repository = repository;
            _userRoleStoreRepository = userRoleStoreRepository;
        }

        public async Task<Result> AddAsync(string UserId, StoreCategoryDto dto)
        {
            if(await _repository.IsCategoryFoundInStore(dto.StoreId, dto.CategoryId)){
              return Result.BadRequest("this Category is exist in store");
            }
            var admin = await _userRoleStoreRepository.GetAdminForStoreAsync(dto.StoreId);
            if (admin == null || admin != UserId)
            {
                return Result.BadRequest("This user is not admin for this store");
            }
            var res = new StoreCategory
            {
                StoreId = dto.StoreId,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(res);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(string UserId,StoreCategoryDto dto)
        {
            var admin = await _userRoleStoreRepository.GetAdminForStoreAsync(dto.StoreId);
            if (admin == null || admin != UserId)
            {
                return Result.BadRequest("This user is not admin for this store");
            }
            var storecategoryId=await _repository.GetStoreCategoryIdByStoreIdAndCategoryId(dto.StoreId,dto.CategoryId);
            if (storecategoryId==null)
            {
                return Result.NotFound("This category not found in this store");
            }
            var res = await _repository.GetByIdAsync(storecategoryId);

            if (res is null)
            {
                return Result.NotFound("Not Found");
            }
            await _repository.DeleteAsync(res);
            return Result.Success();
        }
    }*/
}
