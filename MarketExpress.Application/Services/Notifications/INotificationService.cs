using MarketExpress.Application.Dto.Notification.Request;
using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Notifications
{
    public interface INotificationService
    {
        Task<Result<ResponseAddNotificationDto>> AddAsync(string ownerStoreId, RequestAddNotificationDto request);
        Task<Result<ResponseReadNotificationDto>> MarkAsReadAsync(string userId, int notificationId);
        Task<Result<List<ResponseGetNotificationDto>>> GetAllNotificationAsync(string userId);
        Task<Result<List<ResponseGetNotificationDto>>> GetAllUnReadNotificationAsync(string userId);


    }
}
