using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IUserNotificationRepository:IGenericRepositoryAsync<UserNotification>
    {
        Task<UserNotification?> GetNotificationByUserAsync(string userId, int notificationId);
        Task<List<UserNotification>> GetAllNotificationByUserAsync(string userId);
        Task<List<UserNotification>> GetAllUnReadNotinficationByUserAsync(string userId);
    }
}
