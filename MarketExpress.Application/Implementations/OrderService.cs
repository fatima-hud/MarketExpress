using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
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
        private readonly IStoreRepository _storeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(UserManager<User> userManager, IStoreRepository storeRepository, IOrderRepository orderRepository, ICartItemRepository cartItemRepository, IUserRepository userRepository, ICartRepository cartRepository)
        {
            _userManager = userManager;
            _storeRepository = storeRepository;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
        }

        public async Task<Result<OrderResponseDto>> AddAsync(OrderDto dto,string UserId,int cartId)
        {
            var user= await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return Result<OrderResponseDto>.NotFound("not found userid");
            }
            var Usercart=await _cartRepository.GetByIdAsync(cartId);
            if(Usercart == null || Usercart.UserId != UserId)
            {
                return Result<OrderResponseDto>.BadRequest("this car is not for this user");

            }
          
            var phone=await _userManager.GetPhoneNumberAsync(user);
        /*    var cart= await _cartRepository.GetCartByUserId(user.Id,storeId); 
            var totalPrice = await _cartItemRepository.GetTotalPrice(cart.Id);*/
            var fullname= await _userRepository.GetFullNameAsync(UserId);
            var order = new Order
            {
                UserId = user.Id,
             
                FullName = fullname,
                PhoneNumber = phone,
                Address = dto.Address,
             //    TotalPrice=totalPrice,
                Statue = enOrderStatue.Pending,
                CreatedAt = DateTime.UtcNow
            };
            await _orderRepository.AddAsync(order);
            var res = new OrderResponseDto
            {
                Id = order.Id,
                FullName = order.FullName,
                TotalPrice = order.TotalPrice,
                UserId = user.Id,
              
                PhoneNumber = phone,
                Address = dto.Address,
                Statue = enOrderStatue.Pending,
                CreatedAt = order.CreatedAt

            };
            return Result<OrderResponseDto>.Success(res);




        }

        public async Task<Result<List<Order>>> GetOrdersAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<List<Order>>.NotFound("not found userId");
            }
            var orders= await _orderRepository.GetAllOrder(userId);
            return Result<List<Order>>.Success(orders);
            
        }

        public async Task<Result<OrderResponseDto>> Updatesync(UpdateOrderStatusDto dto,int orderId)
        {
            var order=await _orderRepository.GetByIdAsync(orderId);
            if (order==null)
            {
                return Result<OrderResponseDto>.NotFound("not found");
            }
            order.Statue=dto.status;
            await _orderRepository.UpdateAsync(order);
            var res = new OrderResponseDto
            {
                Id = order.Id,
                FullName = order.FullName,
                TotalPrice = order.TotalPrice,
                UserId = order.UserId,
                StoreId = order.StoreId,
                PhoneNumber = order.PhoneNumber,
                Address = order.Address,
                Statue = order.Statue,
                CreatedAt = order.CreatedAt


            };
            return Result<OrderResponseDto>.Success(res);


        }

    }
}
