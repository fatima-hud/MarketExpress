using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Category.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Dto.Wallet.Request;
using MarketExpress.Application.Dto.Wallet.Response;
using MarketExpress.Application.Services.Wallets;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController>_logger;
        public WalletController(IWalletService walletService, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }
        [HttpGet]
        [Authorize]
        public async Task<ApiResult<ResponseGetWalletDto>> GetWalletAsync()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetWalletDto>.Unauthorized();
                }
                var res = await _walletService.GetWalletAsync(userId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetWalletDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get wallet");
                return ApiResult<ResponseGetWalletDto>.InternalServerError("حدث خطا داخلي");
            }

        }
        [HttpPost]
        [Authorize]
        public async Task<ApiResult<ResponseGetWalletDto>> AddWalletAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetWalletDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetWalletDto>.Unauthorized();
                }
                var res = await _walletService.AddWalletAsync(userId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetWalletDto>.Created(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while add wallet");
                return ApiResult<ResponseGetWalletDto>.InternalServerError("حدث خطا داخلي");
            }
        }
    }
}
