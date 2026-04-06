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
    public class OrderItemRepository : GenericRepositoryAsync<OrderItem>,IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<OrderItem>> GetOrderItemsAsync(int orderId)
        {
            var items= await _dbContext.Orderitems.Where(e=>e.OrderId==orderId).ToListAsync();
            return items;
        }
    }
}
