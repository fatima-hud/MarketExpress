using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Application.Dto.Order.Request;
using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly UserManager<User> _userManager;
     
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IAesEncryptionService _aesEncryptionService;

        public OrderService(UserManager<User> userManager, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository,IAesEncryptionService aesEncryptionService)
        {
            _userManager = userManager;
            
            _orderRepository = orderRepository;
        
          
            _orderItemRepository = orderItemRepository;
            _aesEncryptionService = aesEncryptionService;
        }

        public async Task<Result<List<ResponseGetOrderDto>>> GetAllOrdersAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<List <ResponseGetOrderDto >>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List < ResponseGetOrderDto >>.BadRequest("مستخدم محظور");
            }
            var orders= await _orderRepository.GetAllOrderAsync(userId);
            var res = new List<ResponseGetOrderDto>();
            if (!orders.Any())
            {
                return Result<List<ResponseGetOrderDto>>.Success(res);
            }
          //  var aes = new AesEncryptionService();
            res =orders.Select(order => new ResponseGetOrderDto
            {
                Id = order.Id,
              
                TotalPrice = order.TotalPrice,
                Address =   _aesEncryptionService.Decrypt(order.Address),
                Status = order.Status,
                CreatedAt = order.CreatedAt
            }).ToList();
           
           
            return Result<List<ResponseGetOrderDto>>.Success(res);
            
        }

     

        public async Task<Result<ResponseGetOrderDto>> UpdateOrderStatusAsync(string userId, RequestUpdateOrderStatusDto dto,int orderId)
        {
            var order=await _orderRepository.GetByIdAsync(orderId);
            if (order==null)
            {
                return Result<ResponseGetOrderDto>.NotFound("طلب غير موجود");
            }
            var user= await _userManager.FindByIdAsync(userId);
            if (user==null)
            {
                return Result<ResponseGetOrderDto>.NotFound("مستخدم غير موجود");

            }
            if (user.IsBanned)
            {
                return Result<ResponseGetOrderDto>.BadRequest("مستخدم محظور");
            }
            var superAdminRole = await _userManager.GetRolesAsync(user);
            var isSuperAdmin = superAdminRole.Contains("SuperAdmin");
          
            if (userId != order.UserId&&!isSuperAdmin)
            {
                return Result<ResponseGetOrderDto>.BadRequest("المستخدم غير مصرح له الوصول للطلب");
            }
            if(order.Status!= enOrderStatus.Processing)
            {
                return Result<ResponseGetOrderDto>.BadRequest( $"لا يمكن تعديل الطلب لأنه في حالة {order.Status} بالفعل.");
            }
           if (userId == order.UserId && dto.status != enOrderStatus.Cancelled)
            {
                return Result<ResponseGetOrderDto>.BadRequest("لا يُسمح للمستخدم بتعديل حالة الطلب إلا للإلغاء فقط ");
            }
            /*  if(isSuperAdmin&&dto.status!=enOrderStatus.Rejected)
              {
                  return Result<ResponseGetOrderDto>.BadRequest("يمكن للمسؤول العام تغيير حالة الطلب إلى مرفوض فقط.");
              }*/

            var aes = new AesEncryptionService();
            order.Status=dto.status;
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
