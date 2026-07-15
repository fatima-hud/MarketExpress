using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Delivery.Request;
using MarketExpress.Application.Dto.Delivery.Response;
using MarketExpress.Application.Services.Deliveries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;


        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }
        /*
       [HttpGet]
        [Authorize]
        public async Task<ApiResult<ResponseDeliveryDto>> GetDeliveryByOrderId(int orderId)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseDeliveryDto>.BadRequest(errors);
            }


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseDeliveryDto>.Unauthorized();
            }
            var res = await _deliveryService.GetDeliveryAsync(userId, orderId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);

            }
            return ApiResult<ResponseDeliveryDto>.Ok(res.Value);

        }
        [HttpPost]
        [Authorize(Roles = "Dispatcher")]
        public async Task<ApiResult<ResponseDeliveryDto>> AddDelivery(RequestAddDeliveryDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ApiResult<ResponseDeliveryDto>.BadRequest(errors);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseDeliveryDto>.Unauthorized();
            }

            var res = await _deliveryService.AddDeliveryAsync(userId,request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseDeliveryDto>.Created(res.Value);
        }*/
    }
}
