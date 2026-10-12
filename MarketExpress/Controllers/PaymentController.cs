using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Payment.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Services.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [HttpGet]
        [Authorize]
        public async Task<ApiResult<ResponseGetPaymentDto>> GetPayment(int orderId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetPaymentDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetPaymentDto>.Unauthorized();
                }
                var res = await _paymentService.GetAsync(userId, orderId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetPaymentDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving payment information.");
                return ApiResult<ResponseGetPaymentDto>.InternalServerError("حدث خطا داخلي ");
            }
        }

    } 
}
