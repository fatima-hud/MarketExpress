using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Product.Request;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Services.Categories;
using MarketExpress.Application.Services.Images;
using MarketExpress.Application.Services.Products;
using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;

        public ProductController(IProductService productService, ICategoryService categoryService, IImageService imageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageService = imageService;
        }
/*
        [HttpGet]
        public async Task<ApiResult<List<ResponseProductDto>>> SearchProducts([FromQuery] string query)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));



                return ApiResult<List<ResponseProductDto>>.BadRequest(errors);
            }
            var prod = await _productService.SearchAsync(query);
            if (!prod.IsSuccess)
            {
                return this.ToApiResult(prod);
            }
            return ApiResult<List<ResponseProductDto>>.Ok(prod.Value, "Successful");

        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseProductDto>>> FilterAsync(FilterDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseProductDto>>.BadRequest(errors);
            }
            var products = await _productService.FilterAsync(dto);
            if (!products.IsSuccess)
            {
                return this.ToApiResult(products);
            }
            return ApiResult<List<ResponseProductDto>>.Ok(products.Value, "Successful");

        }
*/

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ApiResult<ResponseProductDetailsDto>> AddProduct([FromForm] RequestAddProductDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseProductDetailsDto>.BadRequest(errors);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseProductDetailsDto>.Unauthorized();
            }
            var res = await _productService.AddAsync(userId, request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseProductDetailsDto>.Created(res.Value);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{productId}")]
        public async Task<ApiResult<ResponseUpdateProductDto>> UpdateProduct([FromRoute] int productId, [FromForm] RequestUpdateProductDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseUpdateProductDto>.BadRequest(errors);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseUpdateProductDto>.Unauthorized();
            }
            var res = await _productService.UpdateAsync(userId, request, productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseUpdateProductDto>.Ok(res.Value);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}")]
        public async Task<ApiResult> DeleteProduct([FromRoute] int productId)
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
            var res = await _productService.DeleteAsync(userId, productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
     /*   [HttpGet("{productId}")]
        public async Task<ApiResult<ResponseProductDetailsDto>> GetProduct(int productId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseProductDetailsDto>.BadRequest(errors);
            }
            var res = await _productService.GetProductAsync(productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseProductDetailsDto>.Ok(res.Value);
        }*/
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetDiscountProductDto>>> GetAllDiscountProduct()
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseGetDiscountProductDto>>.BadRequest(errors);

            }
           

            var res = await _productService.GetAllDiscountProductByAllStores();
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetDiscountProductDto>>.Ok(res.Value);
        }
        [HttpGet("{query}")]
        public async Task<ApiResult<List<ResponseGetDiscountProductDto>>> GetSearch(string query)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseGetDiscountProductDto>>.BadRequest(errors);

            }


            var res = await _productService.SearchAsync(query);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetDiscountProductDto>>.Ok(res.Value);
        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetDiscountProductDto>>> GetFilter(decimal? minPrice,decimal ?maxPrice,enType? type,string? color,enSize? size)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseGetDiscountProductDto>>.BadRequest(errors);

            }
            var request = new FilterDto
            {
                Color = color,
                Size = size,
                Type = type,
                maxPrice = maxPrice,
                minPrice = minPrice
            };

            var res = await _productService.FilterAsync(request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetDiscountProductDto>>.Ok(res.Value);
        }




    }


}
