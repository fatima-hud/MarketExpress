using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.User;
using MarketExpress.Application.Services.Users;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPut]
        public async Task<ApiResult<string>> UpdateProfilePhoto(IFormFile image)
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
        [Authorize]
        [HttpGet]
        public async Task<ApiResult<ResponseUserDto>>GetUserProfile()
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


    }
}



