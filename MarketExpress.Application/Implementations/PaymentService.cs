using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Payment.Request;
using MarketExpress.Application.Dto.Payment.Response;
using MarketExpress.Application.Dto.Rating.Response;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly UserManager<User>_userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public PaymentService(UserManager<User> userManager, IOrderRepository orderRepository, IPaymentRepository paymentRepository,IWalletRepository walletRepository,IOrderItemRepository orderItemRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _walletRepository = walletRepository;
            _orderItemRepository = orderItemRepository;
        }

        

        public async Task<Result<ResponseGetPaymentDto>> GetAsync(string userId, int orderId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result < ResponseGetPaymentDto >.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseGetPaymentDto>.BadRequest("مستخدم محظور");
            }
            var order= await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return Result<ResponseGetPaymentDto>.NotFound("طلب غير موجود");
            
        }
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            if (order.UserId != userId && !isSuperAdmin)
            {
                return Result<ResponseGetPaymentDto>.BadRequest("الطلب لا يخص هذا المستخدم");
            }
            var payment= await _paymentRepository.GetPaymentByOrderIdAsync(orderId);
            if(payment == null)
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
            return Result < ResponseGetPaymentDto>.Success(ret);


            }

       
           
    }
}
