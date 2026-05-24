using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Store.Request;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }





        [HttpGet]
        public async Task<ApiResult<List<ResponseStoreDto>>> GetAllStores()
        {

            var res = await _storeService.GetAllStoresAsync();
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseStoreDto>>.Ok(res.Value);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<ApiResult<ResponseOwnerStoreDto>> GetStoresByAdmin()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseOwnerStoreDto>.Unauthorized();
            }
            var res = await _storeService.GetStoreByAdminAsync(userId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseOwnerStoreDto>.Ok(res.Value);
        }
        [Authorize(Roles ="Admin")]
        [HttpPatch]
        public async Task<ApiResult<ResponseStoreDto>>UpdateStore(RequestUpdateStoreInfoDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseStoreDto>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseStoreDto>.Unauthorized();
            }
            var res=await _storeService.UpdateAsync(userId, request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseStoreDto>.Ok(res.Value);
        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetProductDto>>> GetAllProductsByStore(int storeId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseGetProductDto>>.BadRequest(errors);

            }
            var res=await _storeService.GetAllProductsByStore(storeId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetProductDto>>.Ok(res.Value);


        }









    }
}

