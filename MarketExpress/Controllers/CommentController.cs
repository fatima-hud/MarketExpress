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

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetCommentDto>>> GetAllAsync(int productId,int pageNumber,int pageSize)
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
        [HttpPost]
        public async Task<ApiResult<ResponseAddCommentDto>> AddAsync(RequestAddCommentDto request)
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
        [HttpDelete]
        public async Task<ApiResult> DeleteAsync(int commentId)
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
        [HttpPut]
        public async Task<ApiResult<ResponseAddCommentDto>> UpdateAsync(RequestUpdateCommentDto request)
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
    }
}
