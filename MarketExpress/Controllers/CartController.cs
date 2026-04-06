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
        [HttpPost("AddToCart")]
        public async Task<ApiResult<ResponseCartItemDto>> AddToCart([FromBody]RequestCartItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseCartItemDto>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int Id))
            {
                return ApiResult<ResponseCartItemDto>.Unauthorized();
            }
            var res = await _cartItemService.AddAsync(userId,dto);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseCartItemDto>.Created(res.Value,"sucess");
        }

        [Authorize]
        [HttpPut("UpdateCartItem")]
        public async Task<IActionResult> UpdateCartItem(int cartId, int productId, UpdateCartItemDto dto)
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
            var res = await _cartItemService.UpdateAsync(cartId,productId,dto);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
        [Authorize]
        [HttpDelete("DeleteCartItem/{id}")]
        public async Task<IActionResult> DeleteCartItem(int cartId,List<int> products)
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
            var res = await _cartItemService.DeleteAsync(cartId,products);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }

        [Authorize]
        [HttpGet("GetCartItems")]
        public async Task<IActionResult> GetCartItems(int cartId)
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
            var res = await _cartItemService.GetItemsFromCartIdAsync(cartId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }

    }
}
