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
    public class OrderRepository : GenericRepositoryAsync<Order>,IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Order>> GetAllOrderAsync(string userId)
        {
            var orders=await _dbContext.Orders.Where(e=>e.UserId == userId).OrderByDescending(e=>e.CreatedAt).ToListAsync();
            
            return orders;
            
        }
    }
}
