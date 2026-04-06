using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
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

        public async Task<Result<List<OrderItemResponseDto>>> AddAsync(List<OrderItemDto> dto, string UserId, int OrderId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return Result<List<OrderItemResponseDto>>.NotFound("Not found userId");
            }
            var order = await _orderRepository.GetByIdAsync(OrderId);
            if (order == null)
            {
                return Result<List<OrderItemResponseDto>>.NotFound("Not found order");
            }
            foreach (var item in dto)
            {
                var prod = await _productRepository.GetByIdAsync(item.ProductId);
                if (prod == null)
                {
                    return Result<List<OrderItemResponseDto>>.NotFound("Not found prod");
                }

            }
            var orderItems = new List<OrderItem>();
            foreach (var item in dto)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    OrderId = OrderId,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    Color = item.Color
                };
                orderItems.Add(orderItem);
            }


            await _orderItemRepository.AddRangeAsync(orderItems);
            var res = new List<OrderItemResponseDto>();
            foreach (var item in orderItems)
            {
                var orderItemRepose = new OrderItemResponseDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    OrderId = item.OrderId,
                    UserId = UserId,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    Color = item.Color

                };
                res.Add(orderItemRepose);
            }
            return Result<List<OrderItemResponseDto>>.Success(res);










        }

        public async Task<Result<List<OrderItem>>> GetAllOrderItems(int OrderId)
        {
           var order = await _orderRepository.GetByIdAsync(OrderId);
            if (order == null)
            {
                return Result<List<OrderItem>>.NotFound("not found order");
            }
           var res= await _orderItemRepository.GetOrderItemsAsync(OrderId);
            return Result<List<OrderItem>>.Success(res);
        }
    }
}
