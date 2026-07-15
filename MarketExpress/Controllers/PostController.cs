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

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
     /*   [Authorize(Roles ="SocialMediaManager")]
        [HttpPost]
        public async Task<ApiResult<ResponseGetPostDto>> Add([FromForm] RequestAddPostDto request)
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

        [Authorize(Roles = "SocialMediaManager")]
        [HttpDelete("{postId}")]
        public async Task<ApiResult>Delete( int postId)
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

        [HttpGet("{storeId}")]
        public async Task<ApiResult<List<ResponseGetPostDto>>> GetAll( int storeId)
        {
         
            var res = await _postService.GetAllAsync(storeId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetPostDto>>.Ok(res.Value);
        }
        [Authorize(Roles = "SocialMediaManager")]
        [HttpPut("{postId}")]
        public async Task<ApiResult<ResponseGetPostDto>> Update(int postId,[FromForm] RequestUpdatePostDto request)
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
            var res = await _postService.UpdateAsync(userId,postId,request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseGetPostDto>.Ok(res.Value);
        }

        */
    }
}
