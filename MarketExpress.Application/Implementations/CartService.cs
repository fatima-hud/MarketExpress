using MarketExpress.Application.Abstacts;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<User> _userManager;

        public CartService(ICartRepository cartRepository, UserManager<User> userManager)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

       /* public async Task<Result> AddCartAsync(string userId)
        {
            var user= await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.NotFound("Not Found UserId");
            }
            if (await _cartRepository.IsUserHasCart(userId,storeId))
            {
                return Result.BadRequest("User Has Cart");
            }
            var res = new Cart
            {
                UserId = userId
            };
            await _cartRepository.AddAsync(res);


            return Result.Success();
        }*/
    }
}
