using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPut("UpdateProfilePhoto")]
        public async Task<ApiResult<string>> UpdateProfilePhoto(IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<string>.BadRequest(ModelState.ToString());
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


    }

}
