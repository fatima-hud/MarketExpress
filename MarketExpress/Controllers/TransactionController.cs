using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Transaction.Request;
using MarketExpress.Application.Dto.Transaction.Response;
using MarketExpress.Application.Services.Payments;
using MarketExpress.Application.Services.Transactions;
using MarketExpress.Core.DtoHelping.Order;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionController : ControllerBase

    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;
        private readonly IPaymentService _paymentService;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger, IPaymentService paymentService)
        {
            _transactionService = transactionService;
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost]
        [Authorize(Roles = "EmployeeOfPayment")]
        public async Task<ApiResult<ResponseGetTransactionDto>> AddTransaction(RequestAddTransactionDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetTransactionDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetTransactionDto>.Unauthorized();
                }
                var res = await _transactionService.AddTransactionAsync(userId, request);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetTransactionDto>.Created(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while add tranaction");
                return ApiResult<ResponseGetTransactionDto>.InternalServerError("حدث خطا داخلي");
            }

        }
        [HttpGet]
        [Authorize]
        public async Task<ApiResult<List<ResponseGetTransactionDto>>> GetAllTransactions()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<List<ResponseGetTransactionDto>>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<List<ResponseGetTransactionDto>>.Unauthorized();
                }
                var res = await _transactionService.GetAllTransactionAsync(userId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<List<ResponseGetTransactionDto>>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get all tranactions");
                return ApiResult<List<ResponseGetTransactionDto>>.InternalServerError("حدث خطا داخلي");
            }


        }

        [Authorize]
        [HttpGet("{transactionId}")]
        public async Task<ApiResult<ResponseGetOrderByPaymentDto>> GetOrderDetailsByPayment(int transactionId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<ResponseGetOrderByPaymentDto>.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult<ResponseGetOrderByPaymentDto>.Unauthorized();
                }

                var res = await _paymentService.GetOrderDetailsByPaymentDto(userId,transactionId);
                if (!res.IsSuccess)
                {
                    return this.ToApiResult(res);
                }
                return ApiResult<ResponseGetOrderByPaymentDto>.Ok(res.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get orders by payment");
                return ApiResult<ResponseGetOrderByPaymentDto>.InternalServerError("حدث خطأ داخلي");
            }
        }
    }

}
