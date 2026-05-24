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
    public class DeliveryRepository : GenericRepositoryAsync<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Delivery> GetDeliveryByOrderIdAsync(int orderId)
        {
           var res= await _dbContext.Deliveries.FirstOrDefaultAsync(x => x.OrderId == orderId);
            return res;
        }
    }
}
