using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto;
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
    public class SAdminController : ControllerBase
    {
        private readonly ISAdminService _adminService;


        public SAdminController(ISAdminService adminService)
        {
            _adminService = adminService;
        }

       [Authorize(Roles ="SuperAdmin")]
        [HttpPost("RevokeToken/{userId}")]
        public async Task<ApiResult<bool>> RevokeToken(string userId)
        {

            if (!ModelState.IsValid)
            {
                return ApiResult<bool>.BadRequest(ModelState.ToString());
            }
            var result = await _adminService.RevokeTokenAsync(userId);
            if (!result.IsSuccess)
            {
                return this.ToApiResult(result);//"Token is invalid"

            }
            return ApiResult<bool>.Ok(true);

        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("AddRole")]
        public async Task<ApiResult> AddRole(AddRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var SadminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(SadminId))
            {
                return ApiResult.Unauthorized();
            }

            var res = await _adminService.AddRoleAsync(SadminId, dto);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Created();

        }
    }
}
