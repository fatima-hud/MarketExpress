using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Notification.Request;
using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Application.Services.Notifications;
using MarketExpress.Application.Services.UserNotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;

        public NotificationController(INotificationService notificationService, IUserNotificationService userNotificationService)
        {
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
        }
     /*   [Authorize(Roles = "Admin")]
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
        [Authorize]
        [HttpGet("GetAllNotification")]
        public async Task<ApiResult<List<ResponseGetNotificationDto>>> GetAllNotification()
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<List<ResponseGetNotificationDto>>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseGetNotificationDto>>.Unauthorized();
            }
            var res = await _userNotificationService.GetAllNotificationByUserAsync(userId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetNotificationDto>>.Ok(res.Value);
        }

        [Authorize]
        [HttpGet("GetAllUnReadNotification")]
        public async Task<ApiResult<List<ResponseGetNotificationDto>>> GetAllUnReadNotification()
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<List<ResponseGetNotificationDto>>.BadRequest(ModelState.ToString());
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ApiResult<List<ResponseGetNotificationDto>>.Unauthorized();
            }
            var res = await _userNotificationService.GetAllUnReadNotificationByUserAsync(userId);
            if (!res.IsSuccess)
            {
                return this.ToApiResult(res);
            }
            return ApiResult<List<ResponseGetNotificationDto>>.Ok(res.Value);
        }

        */

    }
}
