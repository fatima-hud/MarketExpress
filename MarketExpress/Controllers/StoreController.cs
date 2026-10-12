using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Store.Request;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Application.Services.Stores;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreController> _logger;

        public StoreController(IStoreService storeService, ILogger<StoreController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }





        [HttpGet]
        public async Task<ApiResult<List<ResponseStoreDto>>> GetAllStores()
        {
            try
            {

                var res = await _storeService.GetAllStoresAsync();
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseStoreDto>>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get all stores");
                return ApiResult<List<ResponseStoreDto>>.InternalServerError("حدث خطا داخلي");
            }
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<ApiResult<ResponseOwnerStoreDto>> GetStoresByAdmin()
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get stores by admin");
                return ApiResult<ResponseOwnerStoreDto>.InternalServerError("حدث خطا داخلي");
            }
        }
        [Authorize(Roles ="Admin")]
        [HttpPatch]
        public async Task<ApiResult<ResponseStoreDto>>UpdateStore(RequestUpdateStoreInfoDto request)
        {
            try
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
                var res = await _storeService.UpdateAsync(userId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseStoreDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while update store");
                return ApiResult<ResponseStoreDto>.InternalServerError("حدث خطا داخلي");
            }
        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetProductDto>>> GetAllProductsByStore(int storeId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetProductDto>>.BadRequest(errors);

                }
                var res = await _storeService.GetAllProductsByStore(storeId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetProductDto>>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get all products by store");
                return ApiResult<List < ResponseGetProductDto >>.InternalServerError("حدث خطا داخلي");
            }


        }
        /*
        [HttpGet]
        [Authorize]
        public async Task<ApiResult<List<ResponseStoreDto>>> GetNearestStores(decimal Latitude, decimal Longitude)
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
            var res = await _storeService.GetNearestStores(userId, Latitude, Longitude);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseStoreDto>>.Ok(res.Value);

        }*/
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ApiResult<ResponseGetStoreImagesDto>>UpdateStoreImages([FromForm]RequestUpdateStoreImagesDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetStoreImagesDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetStoreImagesDto>.Unauthorized();
                }
                var res = await _storeService.UpdateStoreImagesAsync(userId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetStoreImagesDto>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while update store image");
                return ApiResult<ResponseGetStoreImagesDto>.InternalServerError("حدث خطا داخلي");
            }
        }



    } 
    }

