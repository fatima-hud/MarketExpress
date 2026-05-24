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
    public class DeliveryLocationRepository : GenericRepositoryAsync<DeliveryLocation>, IDeliveryLocationRepository
    {
        public DeliveryLocationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<DeliveryLocation>> GetAllLocationByDeliveryAsync(int deliveryId)
        {
            var ret = await _dbContext.DeliveryLocations.Where(e => e.DeliveryId == deliveryId).ToListAsync();
            return ret;
        }
    }
}
