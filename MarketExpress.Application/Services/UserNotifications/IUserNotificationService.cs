using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.UserNotifications
{
    public interface IUserNotificationService
    {
        Task<Result<List<ResponseGetNotificationDto>>> GetAllNotificationByUserAsync(string userId);
        Task<Result<List<ResponseGetNotificationDto>>> GetAllUnReadNotificationByUserAsync(string userId);




    }
}
