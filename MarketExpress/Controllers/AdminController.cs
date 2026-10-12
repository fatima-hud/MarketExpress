using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.ClothingItem.Response;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Role.Request;
using MarketExpress.Application.Services.Admin;
using MarketExpress.Application.Services.Complaints;
using MarketExpress.Application.Services.OrderItems;
using MarketExpress.Application.Services.Payments;
using MarketExpress.Application.Services.ProductColors;
using MarketExpress.Application.Services.Products;
using MarketExpress.Application.Services.Stores;
using MarketExpress.Core.DtoHelping.Admin.Response;
using MarketExpress.Core.DtoHelping.Order;
using MarketExpress.Core.DtoHelping.ProductInventory;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
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
        private readonly IComplaintService _complaintService;
        private readonly ILogger<AdminController> _logger;
        private readonly IPaymentService _paymentService;

        public AdminController(IStoreService storeService, IOrderItemService orderItemService, IProductColorService clothingItemService, IProductService productService, IAdminService adminService,IComplaintService complaintService,ILogger<AdminController> logger,IPaymentService paymentService)
        {
            _storeService = storeService;
            _orderItemService = orderItemService;
            _clothingItemService = clothingItemService;
            _productService = productService;
            _adminService = adminService;
             _complaintService= complaintService;
            _logger=logger;
            _paymentService = paymentService;
        }
       

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{storeId}")]

        public async Task<ApiResult> DeleteAsync(int storeId)
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

                var res = await _storeService.SoftDeleteAsync(userId, storeId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting store {StoreId}", storeId);
                return ApiResult.InternalServerError("حدث خطأ داخلي");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]

        public async Task<ApiResult<List<ResponseGetOrderDetailesDto>>> GetOrdersDetailAsync(DateTime startDate,DateTime endDate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetOrderDetailesDto>>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseGetOrderDetailesDto>>.Unauthorized();
                }

                var res = await _orderItemService.GetProductInventoryAsync(userId, startDate, endDate);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetOrderDetailesDto>>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get Orders Detail");
                return ApiResult<List<ResponseGetOrderDetailesDto>>.InternalServerError("حدث خطأ داخلي");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]

        public async Task<ApiResult<List<ResponseInventoryAlertDto>>> GetProductInventoryAlertAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseInventoryAlertDto>>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseInventoryAlertDto>>.Unauthorized();
                }

                var res = await _clothingItemService.GetAllProductInventoryAlertAsync(userId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseInventoryAlertDto>>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get Product Inventory Alert");
                return ApiResult<List<ResponseInventoryAlertDto>>.InternalServerError("حدث خطأ داخلي");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetDiscountProductByStoreDto>>> GetAllDiscountProductByStore()
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, " Erroe Get All Discount Product By Store");
                return ApiResult<List<ResponseGetDiscountProductByStoreDto>>.InternalServerError("حدث خطأ داخلي");
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]

        public async Task<ApiResult<ResponseProductDashboardDto> >GetProductDashboard(int pageNumber,int pageSize)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseProductDashboardDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseProductDashboardDto>.Unauthorized();
                }

                var res = await _productService.GetProductDashboardsAsync(userId, pageNumber, pageSize);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseProductDashboardDto>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error Get Product Dashboard");
                return ApiResult<ResponseProductDashboardDto>.InternalServerError("حدث خطأ داخلي");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async  Task<ApiResult<ResponseDashboardAnalyticsDto>> GetDashboardAnalyticsAsync( DateTime? fromDate, DateTime? endDate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseDashboardAnalyticsDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseDashboardAnalyticsDto>.Unauthorized();
                }

                var res = await _adminService.GetDashboardAnalyticsAsync(userId, fromDate, endDate);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseDashboardAnalyticsDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get Dashboard Analytics");
                return ApiResult<ResponseDashboardAnalyticsDto>.InternalServerError("حدث خطأ داخلي");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ApiResult<ResponseDashboardSummaryDto>> GetDashboardSummaryAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseDashboardSummaryDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseDashboardSummaryDto>.Unauthorized();
                }

                var res = await _adminService.GetDashboardSummaryAsync(userId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseDashboardSummaryDto>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error Get Dashboard Summery");
                return ApiResult<ResponseDashboardSummaryDto>.InternalServerError("حدث خطأ داخلي");

            }
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

