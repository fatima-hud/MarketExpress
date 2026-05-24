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

        public async Task<CartItem> GetExistingItemAsync(int cartId, int productId, enSize Size, string Color)
        {
            var color = Color.Trim().ToLower();
            var cart = await _dbContext.CartItems.FirstOrDefaultAsync(e => e.ProductId == productId && e.CartId == cartId && e.Size == Size && e.Color == color);
            return cart ;
        }

        public async Task<CartItem> GetCartItemAsync(int cartId, int productId)
        {
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(e => e.ProductId == productId && e.CartId == cartId);
            return cartItem;
        }

        public async Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            var cartItems = await _dbContext.CartItems.Include(e=>e.Product).Where(e => e.CartId == cartId).ToListAsync();
            return cartItems;

            
        }

        public async Task<decimal> GetTotalPriceAsync(int cartId)
        {
            var items = await _dbContext.CartItems
          .Include(e => e.Product)
          .Where(e => e.CartId == cartId)
          .ToListAsync();

            var totalPrice = items.Sum(e =>
                e.Product.PriceAfterDiscount * e.Quantity
            );

            return totalPrice;


        }

        public async Task<List<CartItem>> GetItemsByListIdsAsync(int cartId, List<int> itemsIds)
        {
            var cartItems = await _dbContext.CartItems.Include(e => e.Product).Where(e => e.CartId == cartId && itemsIds.Contains(e.Id)&&!e.Product.IsDeleted).ToListAsync();
           
            return cartItems;
        }

        public async Task<List<CartItem>> GetDetailsCartItemsAsync(int cartId)
        {
            var items = await _dbContext.CartItems.Include(e => e.Product).ThenInclude(e => e.Store).ThenInclude(e => e.Owner).Where(e => e.CartId==cartId).ToListAsync();
            return items;
        }

        public async Task<List<IGrouping<int, CartItem>>>GroupingItemsForStoresAsync(int cartId)
        {
            var res = await _dbContext.CartItems
                .Where(e => e.CartId == cartId)
                .Include(x => x.Product)
                .GroupBy(x => x.Product.StoreId)
                .ToListAsync();

            return res;
        }
    }
}
