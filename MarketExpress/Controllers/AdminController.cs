using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.ClothingItem.Response;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Role.Request;
using MarketExpress.Application.Services.Admin;
using MarketExpress.Application.Services.OrderItems;
using MarketExpress.Application.Services.ProductColors;
using MarketExpress.Application.Services.Products;
using MarketExpress.Application.Services.Stores;
using MarketExpress.Core.DtoHelping.ProductInventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IOrderItemService _orderItemService;
        private readonly IProductColorService _clothingItemService;
        private readonly IProductService _productService;
        private readonly IAdminService _adminService;

        public AdminController(IStoreService storeService, IOrderItemService orderItemService, IProductColorService clothingItemService, IProductService productService, IAdminService adminService)
        {
            _storeService = storeService;
            _orderItemService = orderItemService;
            _clothingItemService = clothingItemService;
            _productService = productService;
            _adminService = adminService;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{storeId}")]

        public async Task<ApiResult> DeleteAsync(int storeId)
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

            var res = await _storeService.SoftDeleteAsync(userId, storeId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]

        public async Task<ApiResult<List<ResponseProductInventoryDto>>> GetProductInventoryAsync(DateTime startDate, DateTime endDate)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseProductInventoryDto>>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseProductInventoryDto>>.Unauthorized();
            }

            var res = await _orderItemService.GetProductInventoryAsync(userId, startDate, endDate);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseProductInventoryDto>>.Ok(res.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]

        public async Task<ApiResult<List<ResponseEmptyProductInventoryDto>>> GetEmptyProductInventoryAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseEmptyProductInventoryDto>>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseEmptyProductInventoryDto>>.Unauthorized();
            }

            var res = await _clothingItemService.GetAllEmptyProductInventoryAsync(userId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseEmptyProductInventoryDto>>.Ok(res.Value);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetDiscountProductByStoreDto>>> GetAllDiscountProductByStore()
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseGetDiscountProductByStoreDto>>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseGetDiscountProductByStoreDto>>.Unauthorized();
            }

            var res = await _productService.GetAllDiscountProductByStore(userId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetDiscountProductByStoreDto>>.Ok(res.Value);
        }
     /*   [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ApiResult> AddRole(RequestAddRoleDto request)
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

            var res = await _adminService.AddRole(userId, request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ApiResult> DeleteRole(RequestAddRoleDto request)
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

            var res = await _adminService.RemoveRole(userId, request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
     */
    }
}

