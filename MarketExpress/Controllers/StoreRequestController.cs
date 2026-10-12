using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Application.Services.StoreRequests;
using MarketExpress.Application.Services.Users;
using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<StoreRequestController> _logger;
        public StoreRequestController(IStoreRequestService storeRequestService, IUserService userService, ILogger<StoreRequestController> logger)
        {
            _storeRequestService = storeRequestService;
            _userService = userService;
            _logger = logger;
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ApiResult<ResponseRequestStoreDto>> AddAsync([FromForm] RequestStoreDto request)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while add store request");
                return ApiResult<ResponseRequestStoreDto>.InternalServerError("حدث خطا داخلي");
            }
        }
        [Authorize]
        [HttpPut("{storeRequestId}/cancel")]
        public async Task<ApiResult> CancelRequest(int storeRequestId)
        {
            try
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
                var result = await _userService.CancelRequestAsync(userId, storeRequestId);
                if (!result.IsSuccess)
                {
                    return this.ToApiResult(result);
                }
                return ApiResult.Ok("تم الغاء الطلب بنجاح");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while occurred cancel request");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }
        }
        [Authorize]
        [HttpPut("{storeId}/update")]
        public async Task<ApiResult<ResponseUpdateRequestStoreDto>> UpdateRequest(int storeId, RequestUpdateRequestStoreDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseUpdateRequestStoreDto>.BadRequest(errors);

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
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while occurred update request");
                return ApiResult<ResponseUpdateRequestStoreDto>.InternalServerError("حدث خطا داخلي");

            }

        }
        [Authorize(Roles = "SuperAdmin,User")]
        [HttpGet("{storeId}")]
        public async Task<ApiResult<ResponseGetFilesByStoreDto>> GetFilesByStoreAsync(int storeId)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while occurred get files by store");
                return ApiResult<ResponseGetFilesByStoreDto>.InternalServerError("حدث خطا داخلي");
            }

        }
        [Authorize]
        [HttpGet]
        public async Task<ApiResult<List<ResponseOwnerStoreDto>>> GetAllRequestStoreByUser()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while occurred get all request store by user");
                return ApiResult<List<ResponseOwnerStoreDto>>.InternalServerError("حدث خطا داخلي");
            }

        }


        [Authorize]
        [HttpGet]
        public async Task<ApiResult<List<ResponseOwnerStoreDto>>> GetFilterRequestStoreByUser([FromQuery] enStoreStatus storeStatus)
        {
            try
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
                var res = await _storeRequestService.GetFilterRequestStoresByUserAsync(userId, storeStatus);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseOwnerStoreDto>>.Ok(res.Value);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while occurred get filter request store by user");
                return ApiResult<List<ResponseOwnerStoreDto>>.InternalServerError("حدث خطا داخلي");
            }
        }


    }
}
