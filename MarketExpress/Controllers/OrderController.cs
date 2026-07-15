using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Checkout.Response;
using MarketExpress.Application.Dto.Order.Request;
using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Application.Services.Checkouts;
using MarketExpress.Application.Services.OrderItems;
using MarketExpress.Application.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly ICheckoutService _checkoutService;

        public OrderController(IOrderService orderService, IOrderItemService orderItemService, ICheckoutService checkoutService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _checkoutService = checkoutService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetOrderDto>>> GetAllOrderAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseGetOrderDto>>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseGetOrderDto>>.Unauthorized();
            }
            var res = await _orderService.GetAllOrdersAsync(userId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetOrderDto>>.Ok(res.Value);
        }
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<ApiResult<List<ResponseOrderItemDto>>> GetOrderItems(int orderId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<List<ResponseOrderItemDto>>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseOrderItemDto>>.Unauthorized();
            }
            var res = await _orderItemService.GetOrderDetails(userId, orderId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseOrderItemDto>>.Ok(res.Value);


        }
        [Authorize]
        [HttpPost]
        public async Task<ApiResult<CheckoutDto>> AddCheckout([FromBody] RequestAddOrderDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<CheckoutDto>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<CheckoutDto>.Unauthorized();
            }
            var checkOut = await _checkoutService.CheckoutAsync(userId,request);
            if (!checkOut.IsSuccess)
            {
                return this.ToApiResult(checkOut);
            }
            return ApiResult<CheckoutDto>.Created(checkOut.Value);

        }
        [Authorize]
        [HttpPut]
        public async Task<ApiResult<ResponseCancelOrderDto>> CancelOrder(int orderId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseCancelOrderDto>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult< ResponseCancelOrderDto>.Unauthorized();
            }
            var cancelOrder = await _checkoutService.CancelOrderAsync(userId, orderId);
            if (!cancelOrder.IsSuccess)
            {
                return this.ToApiResult(cancelOrder);
            }
            return ApiResult<ResponseCancelOrderDto>.Ok(cancelOrder.Value);

        }
        [Authorize(Roles ="SuperAdmin,User")]
        [HttpPut]
        public async Task<ApiResult<ResponseGetOrderDto>>UpdateOrderStatus([FromBody] RequestUpdateOrderStatusDto request,int orderId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseGetOrderDto>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseGetOrderDto>.Unauthorized();
            }
            var order = await _orderService.UpdateOrderStatusAsync(userId,request,orderId);
            if (!order.IsSuccess)
            {
                return this.ToApiResult(order);
            }
            return ApiResult<ResponseGetOrderDto>.Ok(order.Value);
        }

    }
}
