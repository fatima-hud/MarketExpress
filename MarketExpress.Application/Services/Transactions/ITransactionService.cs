using MarketExpress.Application.Dto.Transaction.Request;
using MarketExpress.Application.Dto.Transaction.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Transactions
{
    public interface ITransactionService
    {
        Task<Result<ResponseGetTransactionDto>> AddTransactionAsync(string userId, RequestAddTransactionDto request);
        Task<Result<List<ResponseGetTransactionDto>>> GetAllTransactionAsync(string userId);
    }
}
