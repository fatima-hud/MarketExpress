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
    public class ComplaintRepository : GenericRepositoryAsync<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Complaint>> ?GetAllComplaintByUserIdAsync(string userId)
        {
            var res = await _dbContext.Complaints.Where(e => e.CustomerId == userId || e.StoreOwnerId == userId).ToListAsync();
            return res;
        

    }
    }
}
