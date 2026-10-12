using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Comment.Request;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Services.Comments;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;
        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetCommentDto>>> GetAllAsync(int productId,int pageNumber,int pageSize)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetCommentDto>>.BadRequest(errors);

                }
                var request = new RequestGetAllCommentDto
                {
                    ProductId = productId,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                var res = await _commentService.GetAllAsync(request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetCommentDto>>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving comments.");
                return ApiResult<List<ResponseGetCommentDto>>.InternalServerError("حدث خطا داخلي ");
            }

        }
        [HttpPost]
        public async Task<ApiResult<ResponseAddCommentDto>> AddAsync(RequestAddCommentDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseAddCommentDto>.BadRequest(errors);
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseAddCommentDto>.Unauthorized();
                }
                var res = await _commentService.AddAsync(userId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseAddCommentDto>.Created(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a comment.");
                return ApiResult<ResponseAddCommentDto>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [HttpDelete]
        public async Task<ApiResult> DeleteAsync(int commentId)
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
                var res = await _commentService.DeleteAsync(userId, commentId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a comment.");
                return ApiResult.InternalServerError("حدث خطا داخلي ");
            }
        }
        [HttpPut]
        public async Task<ApiResult<ResponseAddCommentDto>> UpdateAsync(RequestUpdateCommentDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseAddCommentDto>.BadRequest(errors);
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseAddCommentDto>.Unauthorized();
                }
                var res = await _commentService.UpdateAsync(userId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseAddCommentDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a comment.");
                return ApiResult<ResponseAddCommentDto>.InternalServerError("حدث خطا داخلي ");
            }
        }
    }
}
