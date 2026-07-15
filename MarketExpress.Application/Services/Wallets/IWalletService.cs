using MarketExpress.Application.Dto.Wallet.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Wallets
{
    public interface IWalletService
    {
        Task<Result<ResponseGetWalletDto>> AddWalletAsync(string userId);
        Task<Result<ResponseGetWalletDto>> GetWalletAsync(string userId);

    }
}
