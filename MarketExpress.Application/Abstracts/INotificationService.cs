using MarketExpress.Application.Dto.Notification.Request;
using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
    public interface INotificationService
    {
        Task<Result<ResponseAddNotificationDto>> AddAsync(string ownerStoreId, RequestAddNotificationDto request);
        Task<Result<ResponseReadNotificationDto>> MarkAsReadAsync(string userId, int notificationId);
        Task<Result<List<ResponseGetNotificationDto>>> GetAllNotificationAsync(string userId);
        Task<Result<List<ResponseGetNotificationDto>>> GetAllUnReadNotificationAsync(string userId);


    }
}
