using MarketExpress.Application.Dto.Order.Request;
using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
   public interface IOrderService
    {
      
        Task<Result<ResponseGetOrderDto>>UpdateOrderStatusAsync(string userId,RequestUpdateOrderStatusDto dto,int orderId);
        Task<Result<List<ResponseGetOrderDto>>>GetAllOrdersAsync(string userId);
       

    }
}
