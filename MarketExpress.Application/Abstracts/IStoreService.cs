using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.Store.Request;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
   public interface IStoreService
    {
    
      
        Task<Result>SoftDeleteAsync(string adminId,int storeId);

        Task<Result<ResponseOwnerStoreDto>> GetStoreByAdminAsync(string ownerId);

        Task<Result<List<ResponseStoreDto>>> GetAllStoresAsync();
        Task<Result<ResponseStoreDto>> UpdateAsync(string userId, RequestUpdateStoreInfoDto request);
        Task<Result<List<ResponseGetProductDto>>> GetAllProductsByStore(int storeId);
       



    }
}
