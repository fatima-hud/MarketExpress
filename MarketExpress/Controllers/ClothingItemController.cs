using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.ClothingItem.Request;
using MarketExpress.Application.Dto.ClothingItem.Response;
using MarketExpress.Application.Dto.ProductColor.Response;
using MarketExpress.Application.Dto.ProductSize.Request;
using MarketExpress.Application.Dto.ProductSize.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Services.ProductColors;
using MarketExpress.Application.Services.ProductSizes;
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
        private readonly IProductColorService _productColorService;
        private readonly IProductSizeService _productSizeService;

        public ClothingItemController(IProductColorService clothingItemService,IProductSizeService productSizeService)
        {
            _productColorService = clothingItemService;
            _productSizeService = productSizeService;
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<ApiResult<ResponseClothingItemDto>> AddColorforProduct(int productId, [FromForm] RequestAddClothingItemDto request)
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

            var res = await _productColorService.AddAsync(userId, productId,request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseClothingItemDto>.Created(res.Value);
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<ApiResult<List<ResponseGetProductSizeDto>>> AddSizesforProduct(int productColorId, List<RequestProductSizeDto> request)
        {

            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseGetProductSizeDto>>.BadRequest(errors);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseGetProductSizeDto>>.Unauthorized();
            }

            var res = await _productSizeService.AddAsync(userId, productColorId, request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetProductSizeDto>>.Created(res.Value);
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
            var res = await _productColorService.UpdateAsync(userId, clothingItemId, dto);
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
            var res = await _productColorService.GetAllClothingItemByProduct(productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult< List<ResponseGetClothingItemDto >>.Ok(res.Value);
        }
        [HttpGet("{clothingItemId}")]
        public async Task<ApiResult<ResponseGetProductColorDto>>GetAsync(int clothingItemId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseGetProductColorDto>.BadRequest(errors);
            }
            var res = await _productColorService.GetProductColorById(clothingItemId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseGetProductColorDto>.Ok(res.Value);
        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetAllSizeDto>>> GetAllSizeByProductColorAsync(int productId, string color)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult< List<ResponseGetAllSizeDto>>.BadRequest(errors);
            }
            var res = await _productColorService.GetAllSizeByProductColorAsync(productId, color);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetAllSizeDto>>.Ok(res.Value);

        }
    

       

           [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ApiResult<ResponseGetProductSizeDto>> UpdateSizeforProduct( int productSizeId, RequestUpdateProductSizeDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseGetProductSizeDto>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseGetProductSizeDto>.Unauthorized();
            }
            var res = await _productSizeService.UpdateQuantityAsync(userId, productSizeId,request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseGetProductSizeDto>.Ok(res.Value);
        }

        
        [HttpPost]
        public async Task<ApiResult<List<ResponseSuggestProductDto>>> GetSuggestByProductId([FromQuery]RequestSuggestProductDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult< List<ResponseSuggestProductDto>>.BadRequest(errors);

            }
            var res = await _productColorService.GetAllSuggestClothingItemsByProductIdAsync(request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseSuggestProductDto>>.Ok(res.Value);

        }
    }
}
