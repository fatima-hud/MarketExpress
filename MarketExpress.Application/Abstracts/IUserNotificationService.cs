using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
    public interface IUserNotificationService
    {
        Task<Result<List<ResponseGetNotificationDto>>> GetAllNotificationByUserAsync(string userId);
        Task<Result<List<ResponseGetNotificationDto>>> GetAllUnReadNotificationByUserAsync(string userId);




    }
}
