using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreRequestController : ControllerBase
    {
        private readonly IStoreRequestService _storeRequestService;

        public StoreRequestController(IStoreRequestService storeRequestService)
        {
            _storeRequestService = storeRequestService;
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ApiResult<ResponseRequestStoreDto>> AddAsync([FromForm] RequestStoreDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseRequestStoreDto>.Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseRequestStoreDto>.BadRequest(ModelState.ToString());
            }

            var res = await _storeRequestService.AddAsync(userId, request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseRequestStoreDto>.Created(res.Value);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPatch("approve/{requestId}")]
        public async Task<ApiResult<ResponseApprovedStoreRequestDto>> RequestApproved(int requestId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseApprovedStoreRequestDto>.Unauthorized();
            }
            var res = await _storeRequestService.ApprovedAsync(requestId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseApprovedStoreRequestDto>.Ok(res.Value);

        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPatch("RequestReject")]
        public async Task<ApiResult<ResponseRejectedStoreRequestDto>> RequestRejected([FromBody]RequestRejectdDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseRejectedStoreRequestDto>.Unauthorized();
            }
            var res = await _storeRequestService.RejectedAsync(request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseRejectedStoreRequestDto>.Ok(res.Value);

        }
     
    }
}
