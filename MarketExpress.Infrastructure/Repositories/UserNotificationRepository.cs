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

        public async Task<List<UserNotification>> GetAllNotificationByUserAsync(string userId)
        {
            var ret = await _dbContext.UserNotifications.Include(e => e.Notification).ThenInclude(e=>e.Store).Where(e => e.UserId == userId).OrderByDescending(e => e.Notification.CreatedAt).AsNoTracking().ToListAsync();
            return ret;
        }

        public async Task<List<UserNotification>> GetAllUnReadNotinficationByUserAsync(string userId)
        {
            var ret = await _dbContext.UserNotifications.Include(e => e.Notification).ThenInclude(e=>e.Store).Where(e => e.UserId == userId && !e.IsRead).OrderByDescending(e => e.Notification.CreatedAt).AsNoTracking().ToListAsync();
            return ret;
        }

        public async Task<UserNotification?> GetNotificationByUserAsync(string userId, int notificationId)
        {
            var ret=await _dbContext.UserNotifications.FirstOrDefaultAsync(e=>e.UserId==userId && e.NotificationId == notificationId);
            return ret;
        }
    }
}
