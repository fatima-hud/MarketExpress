using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.ClothingItem.Request;
using MarketExpress.Application.Dto.ClothingItem.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClothingItemController : ControllerBase
    {
        private readonly IClothingItemService _clothingItemService;

        public ClothingItemController(IClothingItemService clothingItemService)
        {
            _clothingItemService = clothingItemService;
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<ApiResult<ResponseClothingItemDto>> AddDetailsforProduct(int productId, RequestAddClothingItemDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseClothingItemDto>.BadRequest(errors);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ResponseClothingItemDto>.Unauthorized();
            }

            var res = await _clothingItemService.AddAsync(userId, productId,request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseClothingItemDto>.Created(res.Value);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ApiResult<ResponseClothingItemDto>> UpdateDetailsforProduct( int clothingItemId, RequestUpdateClothingItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseClothingItemDto>.BadRequest(errors);
                    
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ResponseClothingItemDto>.Unauthorized();
            }
            var res = await _clothingItemService.UpdateAsync(userId, clothingItemId, dto);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseClothingItemDto>.Ok(res.Value);
        }

       
      
        [HttpGet("{productId}")]
        public async Task<ApiResult<List<ResponseGetClothingItemDto>>> GetAllAsync(int productId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult< List < ResponseGetClothingItemDto >>.BadRequest(errors);
            }
            var res = await _clothingItemService.GetAllClothingItemByProduct(productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult< List<ResponseGetClothingItemDto >>.Ok(res.Value);
        }
        [HttpGet("{clothingItemId}")]
        public async Task<ApiResult<ResponseGetClothingItemDto>>GetAsync(int clothingItemId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseGetClothingItemDto>.BadRequest(errors);
            }
            var res = await _clothingItemService.GetClothingItemByProduct(clothingItemId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseGetClothingItemDto>.Ok(res.Value);
        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetAllSizeDto>>> GetAllSizeByProductColorAsync(int productId, string color)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult< List<ResponseGetAllSizeDto>>.BadRequest(errors);
            }
            var res = await _clothingItemService.GetAllSizeByProductColorAsync(productId, color);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetAllSizeDto>>.Ok(res.Value);

        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetAllColorDto>>> GetAllColorByProductSizeAsync(int productId, enSize size)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseGetAllColorDto>>.BadRequest(errors);
            }
            var res = await _clothingItemService.GetAllColorByProductSizeAsync(productId, size);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetAllColorDto>>.Ok(res.Value);
        }



        /*
        [HttpPost("GetSuggestByProductId")]
        public async Task<ApiResult<List<ResponseSuggestProductDto>>> GetSuggestByProductId(RequestSuggestProductDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<List<ResponseSuggestProductDto>>.BadRequest(ModelState.ToString());
            }
            var res = await _clothingItemService.GetClothingItemsByProductIdAsync(request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseSuggestProductDto>>.Ok(res.Value);

        }*/
    }
}
