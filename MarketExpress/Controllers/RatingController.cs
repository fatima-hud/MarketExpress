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
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        [Authorize]
        [HttpPost("AddRating")]
        public async Task<IActionResult> AddRating([FromBody]RatingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ResponseClothingItemDto>.Unauthorized();
            }
            var res = await _ratingService.AddRatingAsync(userId,dto);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Created();
        }
        [Authorize]
        [HttpPut("UpdateRating")]
        public async Task<IActionResult> UpdateRating([FromBody]RatingDto dto, int id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ResponseClothingItemDto>.Unauthorized();
            }
            var res = await _ratingService.UpdateRatingAsync(userId,dto, id);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
        [Authorize]
        [HttpDelete("DeleteRating/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ResponseClothingItemDto>.Unauthorized();
            }
            var res = await _ratingService.DeleteRatingAsync(userId,id);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
        [Authorize]
       
        [HttpDelete("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment( int id)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult<ResponseClothingItemDto>.Unauthorized();
            }
            var res = await _ratingService.DeleteCommentAsync(userId, id);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
    }
}
