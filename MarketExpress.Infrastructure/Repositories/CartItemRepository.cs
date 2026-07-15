using MarketExpress.Application.Dto.CartItem.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class CartItemRepository : GenericRepositoryAsync<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<CartItem> GetExistingItemAsync(int cartId, int productSizeId)
        {
          
            var cart = await _dbContext.CartItems.FirstOrDefaultAsync(e => e.ProductSizeId==productSizeId && e.CartId == cartId );
            return cart ;
        }

        /*    public async Task<CartItem> GetCartItemAsync(int cartId, int productId)
            {
                var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(e => e.ProductId == productId && e.CartId == cartId);
                return cartItem;
            }*/

        public async Task<List<ResponseGetCartItemDto>> GetCartItemsByCartIdAsync(int cartId)
        {
            var now = DateTime.UtcNow;

            return await _dbContext.CartItems
                .AsNoTracking()
                .Where(e => e.CartId == cartId)
                .Select(e => new ResponseGetCartItemDto
                {
                   
                    CartItemId = e.Id,
                    ProductSizeId =e.ProductSizeId,

                    ProductId = e.ProductSize.ProductColor.ProductId,

                    Color = e.ProductSize.ProductColor.Color,
                    ColorHexCode=e.ProductSize.ProductColor.ColorHexCode,

                    Size = e.ProductSize.Size,

                    Quantity = e.Quantity,

                    Price = e.ProductSize.ProductColor.Product.Price,

                    PriceAfterDiscount =
                        e.ProductSize.ProductColor.Product.DiscountPercentage.HasValue
                        &&
                        e.ProductSize.ProductColor.Product.DiscountStartDate.HasValue
                        &&
                        e.ProductSize.ProductColor.Product.DiscountEndDate.HasValue
                        &&
                        e.ProductSize.ProductColor.Product.DiscountStartDate <= now
                        &&
                        e.ProductSize.ProductColor.Product.DiscountEndDate >= now
                            ?
                            e.ProductSize.ProductColor.Product.Price
                            -
                            (
                                e.ProductSize.ProductColor.Product.Price
                                *
                                e.ProductSize.ProductColor.Product.DiscountPercentage.Value
                                / 100m
                            )
                            :
                            e.ProductSize.ProductColor.Product.Price,

                    StoreId =
                        e.ProductSize.ProductColor.Product.StoreId,

                    ProductImage =
                        e.ProductSize.ProductColor.Image
                })
                .ToListAsync();
        }




        public async Task<decimal> GetTotalPriceAsync(int cartId)
        {
            var now = DateTime.UtcNow;

            var totalPrice = await _dbContext.CartItems
                .AsNoTracking()
                .Where(e => e.CartId == cartId)
                .SumAsync(e =>
                    (
                        e.ProductSize.ProductColor.Product.DiscountPercentage.HasValue
                        &&
                        e.ProductSize.ProductColor.Product.DiscountStartDate.HasValue
                        &&
                        e.ProductSize.ProductColor.Product.DiscountEndDate.HasValue
                        &&
                        e.ProductSize.ProductColor.Product.DiscountStartDate <= now
                        &&
                        e.ProductSize.ProductColor.Product.DiscountEndDate >= now
                    )
                    ?
                    (
                        e.ProductSize.ProductColor.Product.Price
                        -
                        (
                            e.ProductSize.ProductColor.Product.Price
                            *
                            e.ProductSize.ProductColor.Product.DiscountPercentage.Value
                            / 100m
                        )
                    ) * e.Quantity
                    :
                    e.ProductSize.ProductColor.Product.Price
                    * e.Quantity
                );

            return totalPrice;
        }

       public async Task<List<CartItem>> GetItemsByListIdsAsync(int cartId, List<int> itemsIds)
        {
            var cartItems = await _dbContext.CartItems.Where(e => e.CartId == cartId && itemsIds.Contains(e.Id)).ToListAsync();
           
            return cartItems;
        }

       /* public async Task<List<CartItem>> GetDetailsCartItemsAsync(int cartId)
        {
            var items = await _dbContext.CartItems.Include(e => e.Product).ThenInclude(e => e.Store).ThenInclude(e => e.Owner).Where(e => e.CartId==cartId).ToListAsync();
            return items;
        }*/

        public async Task<List<IGrouping<int, CartItem>>>GroupingItemsForStoresAsync(int cartId)
        {
            var res = await _dbContext.CartItems
                .Where(e => e.CartId == cartId)
                .Include(x => x.ProductSize).ThenInclude(x => x.ProductColor).ThenInclude(x => x.Product)
                .GroupBy(x => x.ProductSize.ProductColor.Product.StoreId)
                .ToListAsync();

            return res;
        }
    }
}
