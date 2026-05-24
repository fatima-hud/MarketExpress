using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.CartItem.Response;
using MarketExpress.Application.Dto.StoreCategory.Request;
using MarketExpress.Application.Dto.StoreCategory.Response;
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

        public StoreCategoryController(IStoreCategoryService storeCategoryService)
        {
            _storeCategoryService = storeCategoryService;
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ApiResult<ResponseGetStoreCategoryDto>> AddAsync(RequestAddStoreCategoryDto dto)
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetStoreCategoryDto>>> GetAllStoreCategoryByAdminAsync()
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
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{storeId}")]
        public async Task<ApiResult<List<ResponseGetStoreCategoryDto>>> GetAllStoreCategoryAsync(int storeId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseGetStoreCategoryDto>>.BadRequest(errors);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseGetStoreCategoryDto>>.Unauthorized();
            }
            var res = await _storeCategoryService.GetAllStoreCategoryAsync(storeId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetStoreCategoryDto>>.Ok(res.Value);


        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{storeCategoryId}")]
        public async Task<ApiResult> DeleteAsync( int storeCategoryId)
        {
            if (!ModelState.IsValid) {
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
    }
    }
