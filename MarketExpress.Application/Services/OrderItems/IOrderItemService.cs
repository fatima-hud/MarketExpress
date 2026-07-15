using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Core.DtoHelping.ProductInventory;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.OrderItems
{
    public interface IOrderItemService
    {
        Task<Result<List<ResponseOrderItemDto>>> GetOrderDetails(string userId, int orderId);
        Task<Result<List<ResponseProductInventoryDto>>> GetProductInventoryAsync(string userId, DateTime? startDate, DateTime? endDate);

    }
}