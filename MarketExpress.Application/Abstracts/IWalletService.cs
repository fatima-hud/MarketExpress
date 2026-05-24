using MarketExpress.Application.Dto.Wallet.Request;
using MarketExpress.Application.Dto.Wallet.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
    public interface IWalletService
    {
        Task<Result<ResponseGetWalletDto>> AddWalletAsync(string userId);
        Task<Result<ResponseGetWalletDto>> GetWalletAsync(string userId);

    }
}
