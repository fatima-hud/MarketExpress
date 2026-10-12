using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Authontication.Response;
using MarketExpress.Application.Dto.Rating.Request;
using MarketExpress.Application.Dto.Rating.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Services.Ratings;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly ILogger<RatingController> _logger;
        public RatingController(IRatingService ratingService, ILogger<RatingController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost]
        public async Task<ApiResult<ResponseRatingDto>> AddRating([FromBody] RequestRatingDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseRatingDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseRatingDto>.Unauthorized();
                }
                var res = await _ratingService.AddRatingAsync(userId, dto);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseRatingDto>.Created(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while add rating");
                return ApiResult<ResponseRatingDto>.InternalServerError("حدث خطا داخلي");
            }
        }
        [Authorize]
        [HttpGet]
       public async  Task<ApiResult<int>> GetProductRatingByUserAsync(int productId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<int>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<int>.Unauthorized();
                }
                var res = await _ratingService.GetProductRatingByUserAsync(userId, productId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<int>.Created(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occured while get product rating by user");
                return ApiResult<int>.InternalServerError("حدث خطا داخلي");
            }
        }


    }
}
