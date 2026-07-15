using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.StoreRequests
{
    public interface IStoreRequestService
    {
        public Task<Result<ResponseRequestStoreDto>> AddAsync(string userId, RequestStoreDto request);

        public Task<Result<ResponseGetFilesByStoreDto>> GetFilesByStoreAsync(string userId, int storeId);
        public Task<Result<List<ResponseOwnerStoreDto>>> GetAllRequestStoreByUserAsync(string userId);
        public Task<Result<List<ResponseOwnerStoreDto>>> GetFilterRequestStoresByUserAsync(string userId, enStoreStatus storeStatus);


    }
}
