using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClothingItemController : ControllerBase
    {
        private readonly IClothingItemService _clothingItemService;

        public ClothingItemController(IClothingItemService clothingItemService)
        {
            _clothingItemService = clothingItemService;
        }
      [Authorize(Roles ="Admin")]

        [HttpPost("AddDetailsforProduct")]
        public async Task<ApiResult<ResponseClothingItemDto>> AddDetailsforProduct(int productId,ClothingItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseClothingItemDto>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int Id))
            {
                return ApiResult<ResponseClothingItemDto>.Unauthorized();
            }

            var res = await _clothingItemService.AddAsync(userId,productId, dto);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseClothingItemDto>.Created(res.Value);
        }
        [Authorize (Roles ="Admin")]
        [HttpPut("UpdateDetailsforProduct")]
        public async Task<ApiResult<ResponseClothingItemDto>> UpdateDetailsforProduct(int productId,int clothingItemId, ClothingItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseClothingItemDto>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int Id))
            {
                return ApiResult<ResponseClothingItemDto>.Unauthorized();
            }
            var res = await _clothingItemService.UpdateAsync(userId,productId,clothingItemId,dto);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseClothingItemDto>.Ok(res.Value);
        }
       
        [HttpGet("GetColorsByProduct")]
        public async Task<ApiResult<List<string>> >GetColorsByProduct(int productId)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<List<string>>.BadRequest(ModelState.ToString());
            }
            var res = await _clothingItemService.GetAllColorAsync(productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<string>>.Ok(res.Value);
        }

        [HttpGet("GetSizesByProduct")]
        public async Task<ApiResult<List<string>>> GetSizesByProduct(int productId)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<List<string>>.BadRequest(ModelState.ToString());
            }
            var res = await _clothingItemService.GetAllSizeAsync(productId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<string>>.Ok(res.Value);
        }

    }
}
