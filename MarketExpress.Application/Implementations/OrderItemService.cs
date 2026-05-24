using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Application.Dto.Order.Request;
using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Core.Entity;
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
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly UserManager<User> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository, UserManager<User> userManager, IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _orderItemRepository = orderItemRepository;
            _userManager = userManager;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public async Task<Result<List<ResponseOrderItemDto>>> GetOrderDetails(string userId, int orderId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<List<ResponseOrderItemDto>>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List<ResponseOrderItemDto>>.BadRequest("مستخدم محظور");
            }
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return Result<List<ResponseOrderItemDto>>.NotFound("الطلب غير موجود");
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            var isSuperAdmin = userRoles.Contains("SuperAdmin");
            if (order.UserId != userId && !isSuperAdmin)
            {
                return Result<List<ResponseOrderItemDto>>.BadRequest("غير مصرح لك الوصول لهذا الطلب");
            }
            var orderItems = await _orderItemRepository.GetOrderItemsAsync(orderId);
            var ret = orderItems.Select(e => new ResponseOrderItemDto
            {
                Id = e.Id,
                UserId = order.UserId,
                OrderId = orderId,
                ProductId = e.ProductId,
                Quantity = e.Quantity,
                Price = e.Price,
                Color = e.Color,
                ColorHex = e.ColorHexCode,
                Size = e.Size


            }).ToList();
            return Result<List<ResponseOrderItemDto>>.Success(ret);

        }




    }
}
