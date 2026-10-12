using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Post.Request;
using MarketExpress.Application.Dto.Post.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Services.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<ApiResult<ResponseGetPostDto>> Add([FromForm] RequestAddPostDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetPostDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetPostDto>.Unauthorized();
                }
                var res = await _postService.AddAsync(userId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetPostDto>.Created(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a post.");
                return ApiResult<ResponseGetPostDto>.InternalServerError("حدث خطا داخلي ");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{postId}")]
        public async Task<ApiResult>Delete( int postId)
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
                var res = await _postService.DeleteAsync(userId, postId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult.Ok(res.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a post.");
                return ApiResult.InternalServerError("حدث خطا داخلي ");
            }
        }
        [AllowAnonymous]
        [HttpGet("{storeId}")]
        public async Task<ApiResult<List<ResponseGetPostDto>>> GetAll( int storeId)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetPostDto>>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var res = await _postService.GetAllAsync(userId, storeId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetPostDto>>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving posts.");
                return ApiResult<List<ResponseGetPostDto>>.InternalServerError("حدث خطا داخلي ");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{postId}")]
        public async Task<ApiResult<ResponseGetPostDto>> Update(int postId,[FromForm] RequestUpdatePostDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetPostDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetPostDto>.Unauthorized();
                }
                var res = await _postService.UpdateAsync(userId, postId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetPostDto>.Ok(res.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a post.");
                return ApiResult<ResponseGetPostDto>.InternalServerError("حدث خطا داخلي ");
            }
        }

        
    }
}
