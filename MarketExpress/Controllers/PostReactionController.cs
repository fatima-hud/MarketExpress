using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.PostReaction.Request;
using MarketExpress.Application.Dto.PostReaction.Response;
using MarketExpress.Application.Services.PostReactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostReactionController : ControllerBase
    {
        private readonly IPostReactionService _postReactionService;
        private readonly ILogger<PostReactionController> _logger;
        public PostReactionController(IPostReactionService postReactionService, ILogger<PostReactionController> logger)
        {
            _postReactionService = postReactionService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost]
        public async Task<ApiResult<ResponseGetPostReactionDto>> AddReaction([FromBody] RequestAddReactionDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetPostReactionDto>.BadRequest(errors);
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetPostReactionDto>.Unauthorized();
                }
                var res = await _postReactionService.AddReactionAsync(userId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetPostReactionDto>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a reaction.");
                return ApiResult<ResponseGetPostReactionDto>.InternalServerError("حدث خطا داخلي ");
            }
        }
    }
}
