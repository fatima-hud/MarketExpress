using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Implementations
{
    public class UserNotificationService : IUserNotificationService
    {
        public Task<Result<List<ResponseGetNotificationDto>>> GetAllNotificationByUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<ResponseGetNotificationDto>>> GetAllUnReadNotificationByUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
