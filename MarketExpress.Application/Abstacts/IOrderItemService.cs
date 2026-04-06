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
    public interface IOrderItemService
    {
        Task<Result<List<OrderItemResponseDto>>> AddAsync(List<OrderItemDto> dto,string userId,int OrderId);
        Task<Result<List<OrderItem>>>GetAllOrderItems(int OrderId); 
        
    }
}