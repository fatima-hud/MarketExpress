using MarketExpress.Core.Entity;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class CartRepository : GenericRepositoryAsync<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Cart> GetCartByUserId(string userId, int storeId)
        {
            var res =await _dbContext.Carts.FirstOrDefaultAsync(e=>e.UserId==userId);
            return res;

        }

       

        public async Task<bool> IsUserHasCart(string userId, int storeId)
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(e=>e.UserId==userId );
            return cart!=null;
        }
    }
}
