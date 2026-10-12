using Azure.Core;
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
namespace MarketExpress.Application.Services.CartItems
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;
        private readonly IProductColorRepository _clothingItemRepository;
        private readonly IProductSizeRepository _productSizeRepository;

        public CartItemService(ICartItemRepository cartItemRepository, ICartRepository cartRepository, IProductRepository productRepository, UserManager<User> userManager, IProductColorRepository clothingItemRepository, IProductSizeRepository productSizeRepository)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userManager = userManager;
            _clothingItemRepository = clothingItemRepository;
            _productSizeRepository = productSizeRepository;
        }

        public async Task<Result<ResponseCartItemDto>> AddAsync(string userId, RequestCartItemDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
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
                var productSize = await _productSizeRepository.GetByIdAsync(dto.ProductSizeId);
                if (productSize is null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCartItemDto>.NotFound("مقاس المنتج غير موجود");
                }
                var productColor = await _clothingItemRepository.GetByIdAsync(productSize.ProductColorId);
                if (productColor is null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCartItemDto>.NotFound("لون المنتج غير موجود");
                }

                var product = await _productRepository.GetByIdAsync(productColor.ProductId);
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
              
              
                         
                var cartItem = await _cartItemRepository.GetExistingItemAsync(userCart.Id,  productSize.Id);

                var productQuantity = productSize.Quantity;
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
                       ProductSizeId=dto.ProductSizeId,
                        Quantity = dto.Quantity,
                        
                    };
                    await _cartItemRepository.AddAsync(cartItem);
                }
               
                var res = new ResponseCartItemDto
                {
                   
                    StoreId = product.StoreId,
                    CartId = cartItem.CartId,
                    ProductId = product.Id,
                    ProductImage=productColor.Image,
                    Size = productSize.Size,
                    Color = productColor.Color,
                    ColorHexCode = productColor.ColorHexCode,
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
            if (user == null || user.IsDeleted)
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
            if (user == null || user.IsDeleted)
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
            
            var totalPrice=await _cartItemRepository.GetTotalPriceAsync(cart.Id);
            var ret = new ResponseGetInfoCartDto
            {
                CartItemDto = cartItems,
                TotalPrice = totalPrice
            };
            return Result<ResponseGetInfoCartDto>.Success(ret);
        }
        public async Task<Result<ResponseUpdateQuantityItemInCartDto>> UpdateAsync(string userId,int cartItemId,  UpdateCartItemDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
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
         
            var productSize = await _productSizeRepository.GetByIdAsync(cartItem.ProductSizeId);
            if (productSize is null)
            {
                return Result<ResponseUpdateQuantityItemInCartDto>.NotFound("المنتج غير موجود");
            }
          //  var productColor = await _clothingItemRepository.GetByIdAsync(productSize.ProductColorId);
      
            var productQuantity=productSize.Quantity;
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
