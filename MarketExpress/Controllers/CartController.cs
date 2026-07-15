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

        public CartController(ICartService cartService, ICartItemService cartItemService)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
        }


        [Authorize]
        [HttpPost]
        public async Task<ApiResult<ResponseCartItemDto>> AddToCart([FromBody] RequestCartItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseCartItemDto>.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ResponseCartItemDto>.Unauthorized();
            }
            var res = await _cartItemService.AddAsync(userId, dto);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseCartItemDto>.Created(res.Value, "sucess");
        }

        [Authorize]
        [HttpPut]
        public async Task<ApiResult<ResponseUpdateQuantityItemInCartDto>> UpdateCartItem(int cartItemId,  UpdateCartItemDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseUpdateQuantityItemInCartDto>.BadRequest(errors);

            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ResponseUpdateQuantityItemInCartDto>.Unauthorized();
            }
            var res = await _cartItemService.UpdateAsync(userId,cartItemId,request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseUpdateQuantityItemInCartDto>.Ok(res.Value);
        }
        [Authorize]
        [HttpDelete]
        public async Task<ApiResult> DeleteCartItem( RequestDeleteItemsFromCartDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult.BadRequest(errors);

            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult.Unauthorized();
            }
            var res = await _cartItemService.DeleteAsync(userId,request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok("Deleted sucessfuly");
        }

        [Authorize]
        [HttpGet]
        public async Task<ApiResult<ResponseGetInfoCartDto>> GetCartItems()
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

    }
}
