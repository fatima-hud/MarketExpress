using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.CartItem.Request;
using MarketExpress.Application.Dto.CartItem.Response;
using MarketExpress.Application.Dto.Category.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Services.CartItems;
using MarketExpress.Application.Services.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ICartItemService cartItemService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
            _logger = logger;
        }


        [Authorize]
        [HttpPost]
        public async Task<ApiResult<ResponseCartItemDto>> AddToCart([FromBody] RequestCartItemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseCartItemDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseCartItemDto>.Unauthorized();
                }
                var res = await _cartItemService.AddAsync(userId, dto);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseCartItemDto>.Created(res.Value, "success");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding item to cart.");
                return ApiResult<ResponseCartItemDto>.InternalServerError("حدث خطا داخلي ");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ApiResult<ResponseUpdateQuantityItemInCartDto>> UpdateCartItem(int cartItemId,  UpdateCartItemDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseUpdateQuantityItemInCartDto>.BadRequest(errors);

                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseUpdateQuantityItemInCartDto>.Unauthorized();
                }
                var res = await _cartItemService.UpdateAsync(userId, cartItemId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseUpdateQuantityItemInCartDto>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating item in cart.");
                return ApiResult<ResponseUpdateQuantityItemInCartDto>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [Authorize]
        [HttpDelete]
        public async Task<ApiResult> DeleteCartItem( RequestDeleteItemsFromCartDto request)
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
                var res = await _cartItemService.DeleteAsync(userId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok("Deleted sucessfuly");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting item from cart.");
                return ApiResult.InternalServerError("حدث خطا داخلي ");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ApiResult<ResponseGetInfoCartDto>> GetCartItems()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetInfoCartDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetInfoCartDto>.Unauthorized();
                }
                var res = await _cartItemService.GetItemsFromCartIdAsync(userId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetInfoCartDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving cart items.");
                return ApiResult<ResponseGetInfoCartDto>.InternalServerError("حدث خطا داخلي ");
            }
        }

    }
}
