using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.DeliveryLocation.Request;
using MarketExpress.Application.Dto.DeliveryLocation.Response;
using MarketExpress.Application.Services.Deliveries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryLocationController : ControllerBase
    {
        private readonly IDeliveryLocationService _deliveryLocationService;

        public DeliveryLocationController(IDeliveryLocationService deliveryLocationService)
        {
            _deliveryLocationService = deliveryLocationService;
        }

     /*   [Authorize]
        [HttpGet]
        public async Task<ApiResult<List<ResponseGetDeliveryLocationDto>>> GetAllLocation(int deliveryId)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<List<ResponseGetDeliveryLocationDto>>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseGetDeliveryLocationDto>>.Unauthorized();
            }
            var res = await _deliveryLocationService.GetAsync(userId, deliveryId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetDeliveryLocationDto>>.Ok(res.Value);

        }
        [HttpPost]
        public async Task<ApiResult<ResponseDeliveryLocationDto>> AddLocation(RequestAddDeliveryLocationDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseDeliveryLocationDto>.BadRequest(ModelState.ToString());
            }
            var res = await _deliveryLocationService.AddAsync(request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseDeliveryLocationDto>.Created(res.Value);

        }*/

    }
}
