using MarketExpress.Application.Dto.Payment.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Payments
{
    public interface IPaymentService
    {

        Task<Result<ResponseGetPaymentDto>> GetAsync(string userId, int orderId);

    }
}
