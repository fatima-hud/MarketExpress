using MarketExpress.Application.Dto.Order.Request;
using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Orders
{
    public interface IOrderService
    {

        Task<Result<ResponseGetOrderDto>> UpdateOrderStatusAsync(string userId, RequestUpdateOrderStatusDto dto, int orderId);
        Task<Result<List<ResponseGetOrderDto>>> GetAllOrdersAsync(string userId);


    }
}
