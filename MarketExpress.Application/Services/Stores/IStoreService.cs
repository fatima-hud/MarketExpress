using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.Store.Request;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Stores
{
    public interface IStoreService
    {


        Task<Result> SoftDeleteAsync(string adminId, int storeId);

        Task<Result<ResponseOwnerStoreDto>> GetStoreByAdminAsync(string ownerId);

        Task<Result<List<ResponseStoreDto>>> GetAllStoresAsync();
        Task<Result<ResponseStoreDto>> UpdateAsync(string userId, RequestUpdateStoreInfoDto request);
        Task<Result<List<ResponseGetProductDto>>> GetAllProductsByStore(int storeId);
  //      Task<Result<List<ResponseStoreDto>>> GetNearestStores(string userId,decimal Latitude,decimal Longitude);
        Task<Result<ResponseGetStoreImagesDto>>UpdateStoreImagesAsync(string userId, RequestUpdateStoreImagesDto request);
       





    }
}
