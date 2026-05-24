using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Application.Implementations;
using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreRequestController : ControllerBase
    {
        private readonly IStoreRequestService _storeRequestService;
        private readonly IUserService _userService;

        public StoreRequestController(IStoreRequestService storeRequestService,IUserService userService)
        {
            _storeRequestService = storeRequestService;
            _userService = userService;
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
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseRequestStoreDto>.BadRequest(errors);

            }

            var res = await _storeRequestService.AddAsync(userId, request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseRequestStoreDto>.Created(res.Value);
        }
        [Authorize]
        [HttpPut("{storeId}/cancel")]
        public async Task<ApiResult> CancelRequest(int storeId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult.Unauthorized();
            }
            var result = await _userService.CancelRequestAsync(userId, storeId);
            if (!result.IsSuccess)
            {
                return this.ToApiResult(result);
            }
            return ApiResult.Ok("Cancelled Successfully");
        }
        [Authorize]
        [HttpPut("{storeId}/update")]
        public async Task<ApiResult<ResponseUpdateRequestStoreDto>> UpdateRequest(int storeId, RequestUpdateRequestStoreDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult< ResponseUpdateRequestStoreDto>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseUpdateRequestStoreDto>.Unauthorized();
            }
            var result = await _userService.UpdateInfoRequestAsync(userId, storeId, request);
            if (!result.IsSuccess)
            {
                return this.ToApiResult(result);
            }
            return ApiResult<ResponseUpdateRequestStoreDto>.Ok(result.Value, "Updated Successfully");

        }
        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("{storeId}")]
        public async Task<ApiResult<ResponseGetFilesByStoreDto>> GetFilesByStoreAsync(int storeId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseGetFilesByStoreDto>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseGetFilesByStoreDto>.Unauthorized();
            }
            var res = await _storeRequestService.GetFilesByStoreAsync(userId, storeId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseGetFilesByStoreDto>.Ok(res.Value);

        }
        [Authorize]
        [HttpGet]
        public async Task<ApiResult<List<ResponseOwnerStoreDto>>> GetAllRequestStoreByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseOwnerStoreDto>>.Unauthorized();
            }
            var res = await _storeRequestService.GetAllRequestStoreByUserAsync(userId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseOwnerStoreDto>>.Ok(res.Value);

        }


        [Authorize]
        [HttpGet]
        public async Task<ApiResult<List<ResponseOwnerStoreDto>>>GetFilterRequestStoreByUser([FromQuery]enStoreStatus storeStatus)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseOwnerStoreDto>>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseOwnerStoreDto>>.Unauthorized();
            }
            var res = await _storeRequestService.GetFilterRequestStoresByUserAsync(userId,storeStatus);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseOwnerStoreDto>>.Ok(res.Value);

        }


    }
}
