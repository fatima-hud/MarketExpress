using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Category.Response;
using MarketExpress.Application.Dto.Product.Request;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Implementations;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
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

        [HttpGet("SearchProducts")]
        public async Task<ApiResult<List<ProductResponse>>> SearchProducts([FromQuery] string query)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<List<ProductResponse>>.BadRequest(ModelState.ToString());
            }
            var prod = await _productService.SearchAsync(query);
            if (!prod.IsSuccess)
            {
                return this.ToApiResult(prod);
            }
            return ApiResult<List<ProductResponse>>.Ok(prod.Value, "Successful");

        }
        [HttpGet("Filter")]
        public async Task<ApiResult<List<ProductResponse>>> FilterAsync(FilterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<List<ProductResponse>>.BadRequest(ModelState.ToString());
            }
            var products = await _productService.FilterAsync(dto);
            if (!products.IsSuccess)
            {
                return this.ToApiResult(products);
            }
            return ApiResult<List<ProductResponse>>.Ok(products.Value, "Successful");

        }
      





        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<ApiResult<ProductResponse>> AddProduct([FromBody]ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ProductResponse>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ProductResponse>.Unauthorized();
            }
            var res = await _productService.AddAsync(userId,request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ProductResponse>.Created(res.Value);
        }
        [Authorize(Roles ="Admin")]
        [HttpPut("{productId}")]
        public async Task<ApiResult<ProductResponse>> UpdateProduct([FromRoute]int productId,[FromBody]UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ProductResponse>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ProductResponse>.Unauthorized();
            }
            var res = await _productService.UpdateAsync(userId,request,productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ProductResponse>.Ok(res.Value);
        }
        [Authorize(Roles ="Admin")]
        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<ApiResult> DeleteProduct([FromRoute]int productId)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult.Unauthorized();
            }
            var res = await _productService.DeleteAsync(userId,productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
        [HttpGet("GetProduct")]
        public async Task<ApiResult<ProductResponse>> GetProduct(int productId)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ProductResponse>.BadRequest(ModelState.ToString());
            }
            var res = await _productService.GetProductAsync(productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ProductResponse>.Ok(res.Value);
        }

    }

}
