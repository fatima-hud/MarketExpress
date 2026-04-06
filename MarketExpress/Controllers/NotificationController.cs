using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Message.Response;
using MarketExpress.Application.Dto.Notification.Request;
using MarketExpress.Application.Dto.Notification.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ApiResult<ResponseAddNotificationDto>> AddNotificationAsync([FromBody] RequestAddNotificationDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseAddNotificationDto>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseAddNotificationDto>.Unauthorized();
            }
            var res = await _notificationService.AddAsync(userId, request);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseAddNotificationDto>.Created(res.Value);
        }
        [Authorize]
        [HttpPut("{notificationId}/read")]
        public async Task<ApiResult<ResponseReadNotificationDto>> MarkAsReadAsync(int notificationId)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<ResponseReadNotificationDto>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<ResponseReadNotificationDto>.Unauthorized();
            }
            var res = await _notificationService.MarkAsReadAsync(userId, notificationId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<ResponseReadNotificationDto>.Ok(res.Value);

        }



        }
}
