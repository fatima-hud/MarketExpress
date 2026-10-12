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
        private readonly ILogger<ClothingItemController> _logger;

        public ClothingItemController(IProductColorService clothingItemService,IProductSizeService productSizeService, ILogger<ClothingItemController> logger)
        {
            _productColorService = clothingItemService;
            _productSizeService = productSizeService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<ApiResult<ResponseClothingItemDto>> AddColorforProduct(int productId, [FromForm] RequestAddClothingItemDto request)
        {
            try
            {


                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseClothingItemDto>.BadRequest(errors);
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseClothingItemDto>.Unauthorized();
                }

                var res = await _productColorService.AddAsync(userId, productId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseClothingItemDto>.Created(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a color for product.");
                return ApiResult<ResponseClothingItemDto>.InternalServerError("حدث خطا داخلي ");
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<ApiResult<List<ResponseGetProductSizeDto>>> AddSizesforProduct(int productColorId, List<RequestProductSizeDto> request)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding sizes for product.");
                return ApiResult<List<ResponseGetProductSizeDto>>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ApiResult<ResponseClothingItemDto>> UpdateDetailsforProduct( int productColorId, RequestUpdateClothingItemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseClothingItemDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseClothingItemDto>.Unauthorized();
                }
                var res = await _productColorService.UpdateAsync(userId, productColorId, dto);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseClothingItemDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating details for product.");
                return ApiResult<ResponseClothingItemDto>.InternalServerError("حدث خطا داخلي ");
            }
        }

       
      
        [HttpGet("{productId}")]
        public async Task<ApiResult<List<ResponseGetClothingItemDto>>> GetAllAsync(int productId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetClothingItemDto>>.BadRequest(errors);
                }
                var res = await _productColorService.GetAllClothingItemByProduct(productId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetClothingItemDto>>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all clothing items for product.");
                return ApiResult<List<ResponseGetClothingItemDto>>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [HttpGet("{clothingItemId}")]
        public async Task<ApiResult<ResponseGetProductColorDto>>GetAsync(int clothingItemId)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting clothing item by ID.");
                return ApiResult<ResponseGetProductColorDto>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetAllSizeDto>>> GetAllSizeByProductColorAsync(int productId, string color)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetAllSizeDto>>.BadRequest(errors);
                }
                var res = await _productColorService.GetAllSizeByProductColorAsync(productId, color);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetAllSizeDto>>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all sizes by product color.");
                return ApiResult<List<ResponseGetAllSizeDto>>.InternalServerError("حدث خطا داخلي ");
            }

        }
    

       

           [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ApiResult<ResponseGetProductSizeDto>> UpdateSizeforProduct( int productSizeId, RequestUpdateProductSizeDto request)
        {
            try
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
                var res = await _productSizeService.UpdateQuantityAsync(userId, productSizeId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetProductSizeDto>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating size for product.");
                return ApiResult<ResponseGetProductSizeDto>.InternalServerError("حدث خطا داخلي ");
            }
        }

        
        [HttpPost]
        public async Task<ApiResult<List<ResponseSuggestProductDto>>> GetSuggestByProductId([FromQuery]RequestSuggestProductDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseSuggestProductDto>>.BadRequest(errors);

                }
                var res = await _productColorService.GetAllSuggestClothingItemsByProductIdAsync(request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseSuggestProductDto>>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting suggested products by product ID.");
                return ApiResult<List<ResponseSuggestProductDto>>.InternalServerError("حدث خطا داخلي ");
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{productColorId}")]
        public async Task<ApiResult> DeleteProductColor(int productColorId)
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
                var res = await _productColorService.DeleteProductColor(userId, productColorId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok("تم الحذف بنجاح");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product color.");
                return ApiResult.InternalServerError("حدث خطا داخلي ");
            }


            }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productSizeId}")]
        public async Task<ApiResult> DeleteProductSize(int productSizeId)
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
                var res = await _productSizeService.DeleteProductSizeAsync(userId, productSizeId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok("تم الحذف بنجاح");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product size.");
                return ApiResult.InternalServerError("حدث خطا داخلي ");
            }



        }

    }
}
