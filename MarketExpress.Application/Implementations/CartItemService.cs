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
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;    

        public CartItemService(ICartItemRepository cartItemRepository, ICartRepository cartRepository, IProductRepository productRepository, UserManager<User> userManager)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userManager = userManager;

        }

        public async Task<Result<ResponseCartItemDto>> AddAsync(string userId,RequestCartItemDto dto)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ResponseCartItemDto>.NotFound("Not Found UserId");
            }
            var trans=await _cartRepository.BeginTransactionAsync();
            try
            {
                var storeId = 1;
               /* if(storeId == null)
                {
                    return Result<ResponseCartItemDto>.NotFound("Not Found Store Id");
                }*/
                if (!await _cartRepository.IsUserHasCart(userId, storeId))
                {
                    var cartUser = new Cart
                    {
                        UserId = userId,
                       
                    };
                   await _cartRepository.AddAsync(cartUser);
                  

                }


                var cart = await _cartRepository.GetCartByUserId(userId,storeId);
                if (cart is null)
                {
                    return Result<ResponseCartItemDto>.NotFound("Not Found CartId");
                }
                var product = await _productRepository.GetByIdAsync(dto.ProductId);
                if (product is null)
                {
                    return Result<ResponseCartItemDto>.NotFound("Not Found ProductId");
                }
                var check = await _cartItemRepository.CheckProductExistAsync(cart.Id, dto.ProductId, dto.Size, dto.Color);
                if (check)
                {
                    return Result<ResponseCartItemDto>.BadRequest("This Product is Found in Cart");
                }
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Size = dto.Size,
                    Color = dto.Color,
                    Quantity = dto.Quantity
                };
                var res = new ResponseCartItemDto
                {
                    
                    UserId=user.Id,
                    StoreId = storeId,
                    CartId=cart.Id,
                    ProductId = cartItem.ProductId,
                    Size = cartItem.Size,
                    Color = cartItem.Color,
                    Quantity = cartItem.Quantity,
                    
                };
                await _cartItemRepository.AddAsync(cartItem);
                return Result<ResponseCartItemDto>.Success(res);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseCartItemDto>.Failure("Internal error", failureType: ResultFailureType.InternalError);

            }

        }

        public async Task<Result> DeleteAsync(int cartId, List<int>products)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart is null)
            {
                return Result.NotFound("Not Found CartId");
            }
       
            foreach (var item in products)
            {
                var product = await _productRepository.GetByIdAsync(item);
                if (product is null )
                {
                    return Result.NotFound($"Not Found ProductId : {item}");
                }
            }
            var cartItems = new List<CartItem>();
            foreach (var item in products)
            {
                var cartItem = await _cartItemRepository.GetCartItemAsync(cartId, item);
                if (cartItem is null)
                {
                    return Result.NotFound("Not Found ProductId By This Cart");
                }
                cartItems.Add(cartItem);
            }


           
          
           
            await _cartItemRepository.DeleteRangeAsync(cartItems);
            return Result.Success();

        }

        public async Task<Result<List<CartItem>>> GetItemsFromCartIdAsync(int cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart is null)
            {
                return Result<List<CartItem>>.NotFound("Not Found CartId");
            }
            var cartItems=await  _cartItemRepository.GetCartItemsByCartId(cartId);
            return Result<List<CartItem>>.Success(cartItems);
            
        }

        public async Task<Result<ResponseCartItemDto>> UpdateAsync(int cartId, int productId, UpdateCartItemDto dto)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart is null)
            {
                return Result<ResponseCartItemDto>.NotFound("Not Found CartId");
            }
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
            {
                return Result<ResponseCartItemDto>.NotFound("Not Found ProductId");
            }
            var cartItem = await _cartItemRepository.GetCartItemAsync(cartId, productId);
            if (cartItem is null)
            {
                return Result<ResponseCartItemDto>.NotFound("Not Found ProductId By This Cart");
            }
            if (product.Quantity < dto.Quantity)
            {
                return Result<ResponseCartItemDto>.BadRequest($"Quantity of product is {product.Quantity}");
            }
            cartItem.Quantity = dto.Quantity;
            await _cartItemRepository.UpdateAsync(cartItem);
            var res = new ResponseCartItemDto
            {
                CartId = cartItem.Id,
                ProductId = productId,
                Price = cartItem.Price,
                Quantity = cartItem.Quantity,
                Size = cartItem.Size,
                Color = cartItem.Color


            };
            return Result<ResponseCartItemDto>.Success(res);
        }
    }
}
