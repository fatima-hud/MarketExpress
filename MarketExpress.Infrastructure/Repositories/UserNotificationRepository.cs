using MarketExpress.Core.Entity;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class UserNotificationRepository : GenericRepositoryAsync<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserNotification?> GetNotificationByUserAsync(string userId, int notificationId)
        {
            var ret=await _dbContext.UserNotifications.FirstOrDefaultAsync(e=>e.UserId==userId && e.NotificationId == notificationId);
            return ret;
        }
    }
}
