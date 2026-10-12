using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Application.Dto.StoreFollower.Request;
using MarketExpress.Application.Dto.StoreFollower.Response;
using MarketExpress.Application.Services.StoreFollowers;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreFollowerController : ControllerBase
    {
        private readonly IStoreFollowerService _storeFollowerService;
        private readonly ILogger<StoreFollowerController> _logger;

        public StoreFollowerController(IStoreFollowerService storeFollowerService, ILogger<StoreFollowerController> logger)
        {
            _storeFollowerService = storeFollowerService;
            _logger = logger;
        }
        [Authorize]
        [HttpPut]

        public async Task<ApiResult<ResponseGetStoreFollowerDto>> StoreFollow(RequestFollowStoreDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetStoreFollowerDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetStoreFollowerDto>.Unauthorized();
                }

                var res = await _storeFollowerService.UpdateFollowAsync(userId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetStoreFollowerDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred while follow store");
                return ApiResult<ResponseGetStoreFollowerDto>.InternalServerError("حدث خطا داخلي");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetProductsFollowerDto>>> GetProductsByFollowerStores()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetProductsFollowerDto>>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseGetProductsFollowerDto>>.Unauthorized();
                }

                var res = await _storeFollowerService.GetProductsOfFollowerStoresByUserAsync(userId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetProductsFollowerDto>>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred whil get products by follower stores");
                return ApiResult<List<ResponseGetProductsFollowerDto>>.InternalServerError("حدث خطا داخلي");
            }
        }
     
        [HttpGet]
        public async Task<ApiResult<int>> GetStoreFollowersCount(int storeId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<int>.BadRequest(errors);
                }
                var res = await _storeFollowerService.GetStoreFollowersCountAsync(storeId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<int>.Ok(res.Value);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred get followers count");
                return ApiResult<int>.InternalServerError("حدث خطا داخلي");
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ApiResult<List<ResponseStoreDto>>> GetStoreFollowByUserAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseStoreDto>>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseStoreDto>>.Unauthorized();
                }
                var res = await _storeFollowerService.GetStoreFollowByUserAsync(userId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseStoreDto>>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred get stores follow");
                return ApiResult<List<ResponseStoreDto>>.InternalServerError("حدث خطا داخلي");
            }

        }

    }
}
