using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.StoreFollower.Request;
using MarketExpress.Application.Dto.StoreFollower.Response;
using MarketExpress.Application.Services.StoreFollowers;
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

        public StoreFollowerController(IStoreFollowerService storeFollowerService)
        {
            _storeFollowerService = storeFollowerService;
        }
        [Authorize]
        [HttpPut]

        public async Task<ApiResult<ResponseGetStoreFollowerDto>> StoreFollow(RequestFollowStoreDto request)
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

        [Authorize]
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetProductsFollowerDto>>> GetProductsByFollowerStores()
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

    }
}
