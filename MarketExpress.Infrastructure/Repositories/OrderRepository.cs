using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
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

        public async Task<List<Order>> GetAllFilterOrdersAsync(enOrderStatus orderStatus,int pageNumber,int pageSize)
        {
            var orders = await _dbContext.Orders.AsNoTracking().Include(o=>o.User).Where(e => e.Status==orderStatus).OrderByDescending(e => e.CreatedAt).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return orders;
        }

        public async Task<List<Order>> GetAllOrderAsync(string userId)
        {
            var orders=await _dbContext.Orders.Where(e=>e.UserId == userId).OrderByDescending(e=>e.CreatedAt).ToListAsync();
            
            return orders;
            
        }

    }
}
