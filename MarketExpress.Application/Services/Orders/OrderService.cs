using MarketExpress.Application.Dto.Order.Request;
using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Application.Services.AesEncryption;
using MarketExpress.Application.Services.AesEncryptions;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly UserManager<User> _userManager;

        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IAesEncryptionService _aesEncryptionService;

        public OrderService(UserManager<User> userManager, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IAesEncryptionService aesEncryptionService)
        {
            _userManager = userManager;

            _orderRepository = orderRepository;


            _orderItemRepository = orderItemRepository;
            _aesEncryptionService = aesEncryptionService;
        }

        public async Task<Result<List<ResponseGetOrderDto>>> GetAllOrdersAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null || user.IsDeleted)
            {
                return Result<List<ResponseGetOrderDto>>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List<ResponseGetOrderDto>>.BadRequest("مستخدم محظور");
            }
            var orders = await _orderRepository.GetAllOrderAsync(userId);
            var res = new List<ResponseGetOrderDto>();
            if (!orders.Any())
            {
                return Result<List<ResponseGetOrderDto>>.Success(res);
            }
            //  var aes = new AesEncryptionService();
            res = orders.Select(order => new ResponseGetOrderDto
            {
                Id = order.Id,

                TotalPrice = order.TotalPrice,
                Address = _aesEncryptionService.Decrypt(order.Address),
                Status = order.Status,
                CreatedAt = order.CreatedAt
            }).ToList();


            return Result<List<ResponseGetOrderDto>>.Success(res);

        }



        public async Task<Result<ResponseGetOrderDto>> UpdateOrderStatusAsync(string userId, RequestUpdateOrderStatusDto dto, int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return Result<ResponseGetOrderDto>.NotFound("طلب غير موجود");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null || user.IsDeleted)
            {
                return Result<ResponseGetOrderDto>.NotFound("مستخدم غير موجود");

            }
            if (user.IsBanned)
            {
                return Result<ResponseGetOrderDto>.BadRequest("مستخدم محظور");
            }
            if (dto is null)
            {
                return Result<ResponseGetOrderDto>.BadRequest("بيانات الطلب غير صحيحة");
            }
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");

            if ( !isSuperAdmin)
            {
                return Result<ResponseGetOrderDto>.BadRequest("المستخدم غير مصرح له الوصول للطلب");
            }
            if (order.Status != enOrderStatus.Processing)
            {
                return Result<ResponseGetOrderDto>.BadRequest($"لا يمكن تعديل الطلب لأنه في حالة {order.Status} بالفعل.");
            }
          /*  if (userId == order.UserId && dto.status != enOrderStatus.Cancelled)
            {
                return Result<ResponseGetOrderDto>.BadRequest("لا يُسمح للمستخدم بتعديل حالة الطلب إلا للإلغاء فقط ");
            }*/
            if (dto.status != enOrderStatus.Delivered)
            {
                return Result<ResponseGetOrderDto>.BadRequest("يمكن للمسؤول العام تغيير حالة الطلب فقط الى تم التوصيل ");
            }

            var aes = new AesEncryptionService();
            order.Status = dto.status;
            await _orderRepository.UpdateAsync(order);
            var res = new ResponseGetOrderDto
            {
                Id = order.Id,

                TotalPrice = order.TotalPrice,
                Address = aes.Decrypt(order.Address),
                Status = order.Status,
                CreatedAt = order.CreatedAt


            };
            return Result<ResponseGetOrderDto>.Success(res);


        }

    }
}
