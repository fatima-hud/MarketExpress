using MarketExpress.Application.Dto.Payment.Response;
using MarketExpress.Core.DtoHelping.Order;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ITransactionRepository _transactionRepository;

        public PaymentService(UserManager<User> userManager, IOrderRepository orderRepository, IPaymentRepository paymentRepository, IWalletRepository walletRepository, IOrderItemRepository orderItemRepository,IStoreRepository storeRepository,ITransactionRepository transactionRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _walletRepository = walletRepository;
            _orderItemRepository = orderItemRepository;
            _storeRepository = storeRepository;
            _transactionRepository = transactionRepository;
        }



        public async Task<Result<ResponseGetPaymentDto>> GetAsync(string userId, int orderId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null || user.IsDeleted)
            {
                return Result<ResponseGetPaymentDto>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseGetPaymentDto>.BadRequest("مستخدم محظور");
            }
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return Result<ResponseGetPaymentDto>.NotFound("طلب غير موجود");

            }
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            if (order.UserId != userId && !isSuperAdmin)
            {
                return Result<ResponseGetPaymentDto>.BadRequest("الطلب لا يخص هذا المستخدم");
            }
            var payment = await _paymentRepository.GetPaymentByOrderIdAsync(orderId);
            if (payment == null)
            {
                return Result<ResponseGetPaymentDto>.NotFound("سجل الدفع غير موجود");
            }
            var ret = new ResponseGetPaymentDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                OrderId = orderId,
                Date = payment.Date,
                WalletId = payment.WalletId
            };
            return Result<ResponseGetPaymentDto>.Success(ret);


        }

        public async Task<Result<ResponseGetOrderByPaymentDto>> GetOrderDetailsByPaymentDto(string userId, int transactionId)
        {
            // 1. التحقق من المستخدم
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.IsDeleted)
            {
                return Result<ResponseGetOrderByPaymentDto>
                    .NotFound("مستخدم غير موجود");
            }

            if (user.IsBanned)
            {
                return Result<ResponseGetOrderByPaymentDto>
                    .BadRequest("مستخدم محظور");
            }

            // 2. جلب Transaction
            var transaction = await _transactionRepository
                .GetByIdAsync(transactionId);

            if (transaction is null)
            {
                return Result<ResponseGetOrderByPaymentDto>
                    .NotFound("سجل المعاملة غير موجود");
            }

            // 3. إذا لم يكن هناك Payment
            // فهذا يعني أنها ليست عملية شراء
            if (!transaction.PaymentId.HasValue)
            {
                return Result<ResponseGetOrderByPaymentDto>
                    .NotFound(
                        "لا يوجد طلب مرتبط بهذه المعاملة، هذه المعاملة ناتجة عن عملية تحويل رصيد");
            }

            var paymentId = transaction.PaymentId.Value;

            // 4. هل المستخدم صاحب متجر؟
            var store = await _storeRepository
                .GetStoreByAdminAsync(userId);

            // =====================================================
            // CASE 1:
            // المستخدم Customer وليس لديه متجر
            // =====================================================

            if (store is null)
            {
                var order = await _paymentRepository
                    .GetOrderDetailsByPaymentDto(
                        paymentId);

                if (order is null)
                {
                    return Result<ResponseGetOrderByPaymentDto>
                        .NotFound("لا يوجد طلب لهذا الدفع");
                }

                return Result<ResponseGetOrderByPaymentDto>
                    .Success(order);
            }

            // =====================================================
            // CASE 2:
            // المستخدم Store Admin
            // =====================================================

            var storeOrder = await _paymentRepository
                .GetOrderDetailsByPaymentDto(
                    paymentId,
                    store.Id);

            if (storeOrder is null)
            {
                return Result<ResponseGetOrderByPaymentDto>
                    .NotFound("لا يوجد طلبات لهذا الدفع في متجرك");
            }

            return Result<ResponseGetOrderByPaymentDto>
                .Success(storeOrder);
        }
    }
}
