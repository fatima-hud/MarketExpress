using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Application.Dto.RequestStore.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public AdminController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{storeId}")]

        public async Task<ApiResult> DeleteAsync(int storeId)
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

            var res = await _storeService.SoftDeleteAsync(userId, storeId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult.Ok();
        }
      

    }
}
