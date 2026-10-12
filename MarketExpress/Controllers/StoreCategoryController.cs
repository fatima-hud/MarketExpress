using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.CartItem.Response;
using MarketExpress.Application.Dto.StoreCategory.Request;
using MarketExpress.Application.Dto.StoreCategory.Response;
using MarketExpress.Application.Services.StoreCategories;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreCategoryController : ControllerBase
    {
        private readonly IStoreCategoryService _storeCategoryService;
        private readonly ILogger<StoreCategoryController> _logger;

        public StoreCategoryController(IStoreCategoryService storeCategoryService, ILogger<StoreCategoryController> logger)
        {
            _storeCategoryService = storeCategoryService;
            _logger = logger;
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ApiResult<ResponseGetStoreCategoryDto>> AddAsync(RequestAddStoreCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetStoreCategoryDto>.BadRequest(errors);
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetStoreCategoryDto>.Unauthorized();
                }
                var res = await _storeCategoryService.AddAsync(userId, dto);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetStoreCategoryDto>.Created(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while add category to store");
                return ApiResult<ResponseGetStoreCategoryDto>.InternalServerError("حدث خطا داخلي");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetStoreCategoryDto>>> GetAllStoreCategoryByAdminAsync()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseGetStoreCategoryDto>>.Unauthorized();
                }

                var res = await _storeCategoryService.GetAllStoreCategoryAsync(userId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetStoreCategoryDto>>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get categories by store");
                return ApiResult<List<ResponseGetStoreCategoryDto>>.InternalServerError("حدث خطا داخلي");
            }
        }
      
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{storeCategoryId}")]
        public async Task<ApiResult> DeleteAsync( int storeCategoryId)
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
                var res = await _storeCategoryService.DeleteAsync(userId, storeCategoryId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while delete category by store");
                return ApiResult.InternalServerError("حدث خطا داخلي");

            }

        }
    }
    }
