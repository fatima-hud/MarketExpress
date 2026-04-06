using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
    public interface IStoreRequestService
    {
        public Task<Result<ResponseRequestStoreDto>> AddAsync(string userId,RequestStoreDto request);
        public Task<Result<ResponseApprovedStoreRequestDto>> ApprovedAsync(int requestId);
        public Task<Result<ResponseRejectedStoreRequestDto>>RejectedAsync(RequestRejectdDto dto);
   //     public Task<Result<List<ResponsePendingStoreDto>>> GetAllPendingStore();
        
    }
}
