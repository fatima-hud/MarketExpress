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
        private readonly ILogger<ProductController> _logger;


        public ProductController(IProductService productService, ICategoryService categoryService, IImageService imageService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageService = imageService;
            _logger = logger;
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
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a product");
                return ApiResult<ResponseProductDetailsDto>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{productId}")]
        public async Task<ApiResult<ResponseUpdateProductDto>> UpdateProduct([FromRoute] int productId, [FromForm] RequestUpdateProductDto request)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a product");
                return ApiResult<ResponseUpdateProductDto>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}")]
        public async Task<ApiResult> DeleteProduct([FromRoute] int productId)
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
                var res = await _productService.DeleteAsync(userId, productId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a product");
                return ApiResult.InternalServerError("حدث خطا داخلي ");
            }
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
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving discounted products.");
                return ApiResult<List<ResponseGetDiscountProductDto>>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [HttpGet("{query}")]
        public async Task<ApiResult<List<ResponseGetDiscountProductDto>>> GetSearch(string query)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for products.");
                return ApiResult<List<ResponseGetDiscountProductDto>>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetDiscountProductDto>>> GetFilter(decimal? minPrice, decimal? maxPrice, enType? type, string? color, enSize? size)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occured while get filter product");
                return ApiResult<List<ResponseGetDiscountProductDto>>.InternalServerError("حدث خطا داخلي");

            }
        }
    }

}
