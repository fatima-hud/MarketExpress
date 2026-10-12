using MarketExpress.Application.Dto.Wallet.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly UserManager<User> _userManager;
        private readonly IWalletRepository _walletRepository;

        public WalletService(UserManager<User> userManager, IWalletRepository walletRepository)
        {
            _userManager = userManager;
            _walletRepository = walletRepository;
        }

        public async Task<Result<ResponseGetWalletDto>> AddWalletAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<ResponseGetWalletDto>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseGetWalletDto>.BadRequest("مستخدم محظور");
            }

            var existingWallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (existingWallet != null)
            {
                return Result<ResponseGetWalletDto>.BadRequest("مستخدم يملك محفظة مسبقا");
            }




            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Balance = 0

            };

            await _walletRepository.AddAsync(wallet);

            var response = new ResponseGetWalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance
            };

            return Result<ResponseGetWalletDto>.Success(response, "تم انشاء المحفظة بنجاح");



        }

        public async Task<Result<ResponseGetWalletDto>> GetWalletAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<ResponseGetWalletDto>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseGetWalletDto>.BadRequest("مستخدم محظور");
            }
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                return Result<ResponseGetWalletDto>.NotFound("مستخدم لا يملك محفظة");
            }
            var response = new ResponseGetWalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance
            };
            return Result<ResponseGetWalletDto>.Success(response);

        }
    }
}
