using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Category.Request;
using MarketExpress.Application.Dto.Category.Response;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Application.Services.Categories;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<ApiResult<ResponseCategory>> AddAsync(RequestAddCategory request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseCategory>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseCategory>.Unauthorized();
                }
                var res = await _categoryService.AddAsync(request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseCategory>.Created(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a category.");
                return ApiResult<ResponseCategory>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{categoryId}")]

        public async Task<ApiResult> DeleteAsync(int categoryId)
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
                var res = await _categoryService.SoftDeleteAsync(categoryId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a category.");
                return ApiResult.InternalServerError("حدث خطا داخلي ");
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponseCategory>>> GetAllAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseCategory>>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseCategory>>.Unauthorized();
                }
                var res = await _categoryService.GetAllCategoryAsync();
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseCategory>>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all categories.");
                return ApiResult<List<ResponseCategory>>.InternalServerError("حدث خطا داخلي ");
            }
        }
        
    }
}
