using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
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
    public class NotificationRepository : GenericRepositoryAsync<Notification>
    {
        public NotificationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

      /*  public async Task<bool> DuplicateCheckAsync(string createdById, string title, string message,enNotificationType type)
        {
            var fromTime = DateTime.UtcNow.AddSeconds(-10);
            var duplicate = await _dbContext.Notifications.AnyAsync(n =>
                n.CreatedById == createdById &&
                n.Title == title &&
                n.Message == message &&
                n.Type == type&&
                n.CreatedAt >= fromTime
            );
            return duplicate;
        }*/
    }
}
