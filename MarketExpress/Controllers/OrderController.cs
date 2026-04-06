using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
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
        [HttpGet("GetOrder")]
        public async Task<IActionResult> GetOrder()
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int Id))
            {
                return ApiResult<ResponseCartItemDto>.Unauthorized();
            }
            var res = await _orderService.GetOrdersAsync(userId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
        [Authorize]
        [HttpGet("GetOrderItems")]
        public async Task<IActionResult> GetOrderItems(int orderId)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int Id))
            {
                return ApiResult<ResponseCartItemDto>.Unauthorized();
            }
            var res = await _orderItemService.GetAllOrderItems(orderId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();


        }
        [Authorize]
        [HttpPost("Checkout")]
        public async Task<IActionResult> AddCheckout([FromBody] string Address,int CartId)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int Id))
            {
                return ApiResult<ResponseCartItemDto>.Unauthorized();
            }
            var catt = await _checkoutService.CheckoutAsync(Address, userId, CartId);
            if (!catt.IsSuccess)
            {
                return this.ToApiResult(catt);
            }
            return ApiResult.Created();
        }
    }
}
