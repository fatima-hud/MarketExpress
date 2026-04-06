using MarketExpress.Application.Dto.Notification.Request;
using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
    public interface INotificationService
    {
        Task<Result<ResponseAddNotificationDto>> AddAsync(string OwnerStoreId, RequestAddNotificationDto request);

    }
}
