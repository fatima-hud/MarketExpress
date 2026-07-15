using MarketExpress.Application.Dto;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.SuperAdmins
{
    public interface ISuperAdminService
    {
        Task<Result<bool>> RevokeTokenAsync(string userId);
        Task<Result> AddRoleAsync(string SadminId, AddRoleDto dto);



        public Task<Result<ResponseApprovedStoreRequestDto>> ApprovedStoreRequestAsync(int requestId);
        public Task<Result<ResponseRejectedStoreRequestDto>> RejectedStoreRequestAsync(RequestRejectdDto dto);
        public Task<Result<List<ResponsePendingStoreDto>>> GetAllPendingRequestStore();
        public Task<Result<List<ResponsePendingStoreDto>>> GetAllRequestStoreByFilter(enStoreStatus storeStatus, int pageNumber, int pageSize);

    }
}
