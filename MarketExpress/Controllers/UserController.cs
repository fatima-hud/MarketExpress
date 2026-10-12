using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.User;
using MarketExpress.Application.Services.Users;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize]
        [HttpPut]
        public async Task<ApiResult<string>> UpdateProfilePhoto(IFormFile image)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<string>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<string>.Unauthorized();
                }
                var result = await _userService.UpdateProfilePhoto(userId, image);

                if (result.IsSuccess)
                {
                    return ApiResult<string>.Ok(result.Value);
                }
                else
                    return this.ToApiResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while update profile photo");
                return ApiResult<string>.InternalServerError("حدث خطا داخلي");
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ApiResult<ResponseUserDto>>GetUserProfile()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseUserDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseUserDto>.Unauthorized();
                }
                var result = await _userService.GetUserAsync(userId);

                if (!result.IsSuccess)
                {
                    return this.ToApiResult(result);

                }
                return ApiResult<ResponseUserDto>.Ok(result.Value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get profile ");
                return ApiResult<ResponseUserDto>.InternalServerError("حدث خطا داخلي");
            }
                
        }
        [Authorize]
        [HttpPut]
        public async Task<ApiResult<ResponseUserDto>> UpdateUserAsync(RequestUpdateUserDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseUserDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseUserDto>.Unauthorized();
                }
                var res=await _userService.UpdateUserAsync(userId,request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseUserDto>.Ok(res.Value);
            }
            catch(Exception ex)
            {

                _logger.LogError(ex, "Error occurred while update profile ");
                return ApiResult<ResponseUserDto>.InternalServerError("حدث خطا داخلي");
            }
        }


    }
}



