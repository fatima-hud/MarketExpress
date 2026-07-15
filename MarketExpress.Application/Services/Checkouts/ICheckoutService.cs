using MarketExpress.Application.Dto.Checkout.Response;
using MarketExpress.Application.Dto.Order.Request;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Checkouts
{
    public interface ICheckoutService
    {
       Task<Result<CheckoutDto>> CheckoutAsync(string userId, RequestAddOrderDto request);
        Task<Result<ResponseCancelOrderDto>>CancelOrderAsync(string userId,int orderId);    
    }
}
