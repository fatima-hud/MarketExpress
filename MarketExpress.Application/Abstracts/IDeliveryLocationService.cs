using MarketExpress.Application.Dto.DeliveryLocation.Request;
using MarketExpress.Application.Dto.DeliveryLocation.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Abstracts
{
    public interface IDeliveryLocationService
    {
        Task<Result<ResponseDeliveryLocationDto>> AddAsync(RequestAddDeliveryLocationDto request);
        Task<Result<List<ResponseGetDeliveryLocationDto>>> GetAsync(string userId,int deliveryId);
    }
}
