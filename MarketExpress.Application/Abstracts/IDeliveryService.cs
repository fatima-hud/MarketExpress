using MarketExpress.Application.Dto.Delivery.Request;
using MarketExpress.Application.Dto.Delivery.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Abstracts
{
    public interface IDeliveryService
    {
        Task<Result<ResponseDeliveryDto>> AddDeliveryAsync(string dispatcherId,RequestAddDeliveryDto request);
        Task<Result<ResponseDeliveryDto>> GetDeliveryAsync(string UserId, int orderId);

    }
}
