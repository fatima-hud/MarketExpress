using MarketExpress.Core.Entity;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class CartItemRepository : GenericRepositoryAsync<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckProductExistAsync(int cartId, int productId, string Size, string Color)
        {
            var cart = await _dbContext.CartItems.Where(e => e.ProductId == productId && e.CartId == cartId && e.Size == Size && e.Color == Color).FirstOrDefaultAsync();
            return cart != null;
        }

        public async Task<CartItem> GetCartItemAsync(int cartId, int productId)
        {
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(e => e.ProductId == productId && e.CartId == cartId);
            return cartItem;
        }

        public async Task<List<CartItem>> GetCartItemsByCartId(int cartId)
        {
            var cartItems = await _dbContext.CartItems.Where(e => e.CartId == cartId).ToListAsync();
            return cartItems;

            
        }

        public async Task<decimal> GetTotalPrice(int cartId)
        {
            var totalPrice = await _dbContext.CartItems
       .Include(e => e.Product)
       .Where(e => e.CartId == cartId)
       .SumAsync(prod =>
           (prod.Product.DiscountPercentage != null
               ? prod.Product.PriceAfterDiscount
               : prod.Product.Price) ?? 0
       );

            return totalPrice;


        }


    }
}
