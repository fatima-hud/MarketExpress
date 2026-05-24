using Azure.Core;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.CartItem.Request;
using MarketExpress.Application.Dto.CartItem.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;
namespace MarketExpress.Application.Implementations
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;
        private readonly IClothingItemRepository _clothingItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository, ICartRepository cartRepository, IProductRepository productRepository, UserManager<User> userManager, IClothingItemRepository clothingItemRepository)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userManager = userManager;
            _clothingItemRepository = clothingItemRepository;
        }

        public async Task<Result<ResponseCartItemDto>> AddAsync(string userId, RequestCartItemDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ResponseCartItemDto>.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseCartItemDto>.BadRequest("المستخدم محظور");
            }
            var trans = await _cartRepository.BeginTransactionAsync();
            try
            {
                var userCart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (userCart is null)
                {
                    userCart = new Cart
                    {
                        UserId = userId,
                    };
                    await _cartRepository.AddAsync(userCart);
                }
              
                var product = await _productRepository.GetByIdAsync(dto.ProductId);
                if (product is null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCartItemDto>.NotFound("المنتج غير موجود");
                }
                if (product.IsDeleted)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCartItemDto>.BadRequest("المنتج محذوف مسبقا");
                }
                if (dto.Quantity <= 0)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCartItemDto>.BadRequest($"الكمية يجب ان تكون اكبر من 1");
                }
               
               
               
                if (string.IsNullOrEmpty(dto.Color))
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCartItemDto>.BadRequest($"اللون مطلوب");
                }
                if (string.IsNullOrWhiteSpace(dto.ColorHexCode))
                {
                    return Result < ResponseCartItemDto >.BadRequest("رمز اللون مطلوب");
                }
                if (!dto.ColorHexCode.StartsWith("#"))
                {
                    return Result<ResponseCartItemDto>.BadRequest("رمز اللون غير صحيح");
                }
                var item=await _clothingItemRepository.GetClothingItemByProductAsync(product.Id,dto.Size,dto.Color);
                if (item is null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCartItemDto> .NotFound("حجم او لون المنتج غير متوفر");
                       
                }
              
                var cartItem = await _cartItemRepository.GetExistingItemAsync(userCart.Id, dto.ProductId, dto.Size, dto.Color);

                var productQuantity = item.Quantity;
                if (cartItem != null)
                {

                    var newQuantity = cartItem.Quantity + dto.Quantity;

                    if (productQuantity < newQuantity)
                    {
                        await trans.RollbackAsync();

                        return Result<ResponseCartItemDto>.BadRequest(
                            $"الكمية المتوفرة ({productQuantity}) اقل من الطلب ");
                    }

                    cartItem.Quantity = newQuantity;

                    await _cartItemRepository.UpdateAsync(cartItem);
                }
                else
                {
                    if (productQuantity < dto.Quantity)
                    {
                        await trans.RollbackAsync();

                        return Result<ResponseCartItemDto>.BadRequest(  $"الكمية المتوفرة({productQuantity}) اقل من الطلب");
                          
                    }
                    cartItem = new CartItem
                    {
                        CartId = userCart.Id,
                        ProductId = dto.ProductId,
                        Size = dto.Size,
                        Color = dto.Color,
                        Quantity = dto.Quantity,
                        ColorHexCode = dto.ColorHexCode
                    };
                    await _cartItemRepository.AddAsync(cartItem);
                }
                var res = new ResponseCartItemDto
                {
                    UserId = user.Id,
                    StoreId = product.StoreId,
                    CartId = cartItem.CartId,
                    ProductId = cartItem.ProductId,
                    ProductImage=product.Image,
                    Size = cartItem.Size,
                    Color = cartItem.Color,
                    ColorHexCode = cartItem.ColorHexCode,
                    Quantity = cartItem.Quantity,
                    Price=product.Price,
                    PriceAfterDiscount=product.PriceAfterDiscount
                   
                    
                  
                    
                };
                await trans.CommitAsync();
                return Result<ResponseCartItemDto>.Success(res);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseCartItemDto>.Failure("خطا داخلي", failureType: ResultFailureType.InternalError);
            }
        }
        public async Task<Result> DeleteAsync(string userId, RequestDeleteItemsFromCartDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result.BadRequest("المستخدم محظور");
            }
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
         
            if (cart is null)
            {
                return Result.NotFound("السلة غير موجودة");
            }
            if (userId != cart.UserId)
            {
                return Result.BadRequest("لا يسمح للمستخدم الوصول الى هذه السلة");
            }
            var cartItems = await _cartItemRepository.GetItemsByListIdsAsync(cart.Id, request.cartItemIds);
            if (request.cartItemIds == null || !request.cartItemIds.Any())
            {
                return Result.BadRequest("لا يوجد عناصر في السلة");
            }
            if (cartItems.Count != request.cartItemIds.Count)
            {
                return Result.NotFound("بعض العناصر غير موجودة في السلة ");
            }
            await _cartItemRepository.DeleteRangeAsync(cartItems);
            return Result.Success();
        }
        public async Task<Result<ResponseGetInfoCartDto> >GetItemsFromCartIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ResponseGetInfoCartDto>.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseGetInfoCartDto>.BadRequest("المستخدم محظور");
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart is null)
            {
                return Result<ResponseGetInfoCartDto>.NotFound("السلة غير موجودة");
            }
            if (cart.UserId != userId)
            {
                return Result<ResponseGetInfoCartDto>.BadRequest("لا يسمح للمستخدم الوصول الى هذه السلة");
            }
            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cart.Id);
            var retCartItem = cartItems.Select(e => new ResponseGetCartItemDto
            {
                StoreId = e.Product.StoreId,
                ProductId = e.ProductId,
                ProductImage = e.Product.Image,
                Size = e.Size,
                Color = e.Color,
                Quantity = e.Quantity,
                Price = e.Product.Price,
                PriceAfterDiscount = e.Product.PriceAfterDiscount

                
            }).ToList();
            var totalPrice=await _cartItemRepository.GetTotalPriceAsync(cart.Id);
            var ret = new ResponseGetInfoCartDto
            {
                CartItemDto = retCartItem,
                TotalPrice = totalPrice
            };
            return Result<ResponseGetInfoCartDto>.Success(ret);
        }
        public async Task<Result<ResponseUpdateQuantityItemInCartDto>> UpdateAsync(string userId,int cartItemId,  UpdateCartItemDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ResponseUpdateQuantityItemInCartDto>.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseUpdateQuantityItemInCartDto>.BadRequest("المستخدم محظور");
            }
            
           
            var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
            if (cartItem is null)
            {
                return Result<ResponseUpdateQuantityItemInCartDto>.NotFound("العنصر غير موجود في السلة");
            }
            var cart = await _cartRepository.GetByIdAsync(cartItem.CartId);
            if (cart is null)
            {
                return Result<ResponseUpdateQuantityItemInCartDto>.NotFound("السلة غير موجودة");
            }

            if (cart.UserId != userId)
            {
                return Result<ResponseUpdateQuantityItemInCartDto>.BadRequest("لا يسمح للمستخدم الوصول الى هذه السلة");
            }
            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
            if (product is null)
            {
                return Result<ResponseUpdateQuantityItemInCartDto>.NotFound("المنتج غير موجود");
            }
            var item = await _clothingItemRepository.GetClothingItemByProductAsync(cartItem.ProductId, cartItem.Size, cartItem.Color);
            var productQuantity=item.Quantity;
            if (request.Quantity <= 0)
            {
                return Result<ResponseUpdateQuantityItemInCartDto>.BadRequest($"الكمية يجب ان تكون اكبر من صفر");
            }
            if (productQuantity < request.Quantity)
            {
                return Result<ResponseUpdateQuantityItemInCartDto>.BadRequest($"الكمية المتوفرة من المنتج{productQuantity} اقل من الطلب ");
            }
            cartItem.Quantity = request.Quantity;
            await _cartItemRepository.UpdateAsync(cartItem);
            var res = new ResponseUpdateQuantityItemInCartDto
            {
                CartItemId = cartItem.Id,
                Quantity = cartItem.Quantity
            };
            return Result<ResponseUpdateQuantityItemInCartDto>.Success(res);
        }
    }
}
