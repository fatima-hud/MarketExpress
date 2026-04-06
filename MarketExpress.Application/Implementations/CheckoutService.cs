using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class CheckoutService : ICheckoutService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly OrderService _orderService;
        private readonly OrderItemService _orderItemService;
        private readonly IOrderRepository _orderRepository;

        public CheckoutService(UserManager<User> userManager, ICartRepository cartRepository, ICartItemRepository cartItemRepository, OrderService orderService, OrderItemService orderItemService, IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _orderRepository = orderRepository;
        }

        public async Task<Result<CheckoutDto>> CheckoutAsync(string userId,string Address,int storeId)
        {
            var user =await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<CheckoutDto>.NotFound("not found user");
            }
            var cart= await _cartRepository.GetCartByUserId(userId,storeId);
            if (cart == null)
            {
                return Result<CheckoutDto>.NotFound("not found cart");
            }
            var orderdto = new OrderDto
            {
                Address = Address
            };
            var order = await _orderService.AddAsync(orderdto, userId, storeId);
            var orderItems=new List<OrderItemDto>();
            var cartItems=await _cartItemRepository.GetCartItemsByCartId(cart.Id);
            foreach (var cartItem in cartItems) {
                var orderItemDto = new OrderItemDto
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Color = cartItem.Color,
                    Size = cartItem.Size
                };
                orderItems.Add(orderItemDto);
            }



            var orderItem = await _orderItemService.AddAsync(orderItems, userId, order.Value.Id );
            var GetOrder=await _orderRepository.GetByIdAsync(order.Value.Id);
            var TotalPrice= await _cartItemRepository.GetTotalPrice(cart.Id);
            order.Value.TotalPrice = TotalPrice;
            GetOrder.TotalPrice = TotalPrice;
            await _orderRepository.UpdateAsync(GetOrder);
            var res = new CheckoutDto
            {
                Order = order.Value,
                Items = orderItem.Value


            };
            return Result<CheckoutDto>.Success(res);



        }
    }
}
