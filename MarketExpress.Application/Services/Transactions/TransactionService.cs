using MarketExpress.Application.Dto.Transaction.Request;
using MarketExpress.Application.Dto.Transaction.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;

        public TransactionService(UserManager<User> userManager, ITransactionRepository transactionRepository, IWalletRepository walletRepository)
        {
            _userManager = userManager;
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
        }

        public async Task<Result<ResponseGetTransactionDto>> AddTransactionAsync(string employeeId, RequestAddTransactionDto request)
        {
            var employee = await _userManager.FindByIdAsync(employeeId);
            if (employee == null || employee.IsDeleted)
            {
                return Result<ResponseGetTransactionDto>.NotFound("مستخدم غير موجود");
            }
            if (employee.IsBanned)
            {
                return Result<ResponseGetTransactionDto>.BadRequest("مستخدم محظور");
            }
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
            if (wallet == null)
            {
                return Result<ResponseGetTransactionDto>.NotFound("لا يوجد محفظة");
            }
            if (request.Amount <= 0)
            {
                return Result<ResponseGetTransactionDto>.BadRequest("الكمية يجب ان تكون اكبر من 0");
            }
            wallet.Balance += request.Amount;
            await _transactionRepository.SaveChangesAsync();

            var transaction = new TransactionProcess
            {
                WalletId = request.WalletId,
                Amount = request.Amount,
                TransactionType = enTransactionType.Deposit,
                Date = DateTime.UtcNow,

            };
            await _transactionRepository.AddAsync(transaction);
            var ret = new ResponseGetTransactionDto
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                Date = transaction.Date

            };
            return Result<ResponseGetTransactionDto>.Success(ret);

        }

        public async Task<Result<List<ResponseGetTransactionDto>>> GetAllTransactionAsync(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<List<ResponseGetTransactionDto>>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List<ResponseGetTransactionDto>>.BadRequest("مستخدم محظور");
            }
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                return Result<List<ResponseGetTransactionDto>>.NotFound("لا يوجد محفظة");

            }
            var transactions = await _transactionRepository.GetAllAsync(wallet.Id);
            var ret = new List<ResponseGetTransactionDto>();
            if (transactions.Count == 0)
            {
                return Result<List<ResponseGetTransactionDto>>.Success(ret);
            }
            ret = transactions.Select(e => new ResponseGetTransactionDto
            {
                Id = e.Id,
                WalletId = e.WalletId,
                Amount = e.Amount,
                PaymentId = e.PaymentId,
                TransactionType = e.TransactionType,
                Date = e.Date
            }).ToList();
            return Result<List<ResponseGetTransactionDto>>.Success(ret);







        }
    }
}
