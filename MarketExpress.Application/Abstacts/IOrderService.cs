using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
   public interface IOrderService
    {
        Task<Result<OrderResponseDto>>AddAsync(OrderDto dto,string UserId,int StoreId);
        Task<Result<OrderResponseDto>>Updatesync(UpdateOrderStatusDto dto,int orderId);
        Task<Result<List<Order>>>GetOrdersAsync(string userId);

    }
}
