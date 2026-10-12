using MarketExpress.Application.Dto.Payment.Response;
using MarketExpress.Core.DtoHelping.Order;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Payments
{
    public interface IPaymentService
    {

        Task<Result<ResponseGetPaymentDto>> GetAsync(string userId, int orderId);
        Task<Result<ResponseGetOrderByPaymentDto>> GetOrderDetailsByPaymentDto(string userId,int transactionId);

    }
}
