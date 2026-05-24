using MarketExpress.Application.Dto.Transaction.Request;
using MarketExpress.Application.Dto.Transaction.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
  public interface ITransactionService 
    {
        Task<Result<ResponseGetTransactionDto>> AddTransactionAsync(string userId,RequestAddTransactionDto request);
        Task<Result<List<ResponseGetTransactionDto>>>GetAllTransactionAsync(string userId);
    }
}
