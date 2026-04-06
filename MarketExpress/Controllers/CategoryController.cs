using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Category.Request;
using MarketExpress.Application.Dto.Category.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }
        [Authorize(Roles ="SuperAdmin")]
        [HttpPost("Add")]
        public async Task<ApiResult<CategoryResponse>> AddAsync(AddCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<CategoryResponse>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<CategoryResponse>.Unauthorized();
            }
            var res = await _categoryService.AddAsync(request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<CategoryResponse>.Created(res.Value);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("Delete/{id}")]

        public async Task<ApiResult> DeleteAsync(DeleteCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult.Unauthorized();
            }
            var res = await _categoryService.SoftDeleteAsync(request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
    }
}
