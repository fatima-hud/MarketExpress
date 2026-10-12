using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Core.DtoHelping.Complaint;
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

        public async Task<List<Complaint>> GetAllComplaintByAdminAsync(string adminId)
        {
            var res = await _dbContext.Complaints
                .Include(e=>e.Customer)
                .Include(e=>e.Messages)
                .Where(e =>  e.StoreOwnerId == adminId)
               .OrderByDescending(e => e.Messages
            .Select(m => (DateTime?)m.SentAt)
            .Max())
                .ToListAsync();

            return res;
        }
        public async Task<List<ResponseGetComplaintByUserDto>> GetAllComplaintByUserAsync(string userId)
        {
            return await _dbContext.Complaints
                .Where(c => c.CustomerId == userId)
                .Select(c => new ResponseGetComplaintByUserDto
                {
                    ComplaintId = c.Id,

                    StoreId = c.StoreOwner.Stores
                        .Where(s => s.IsActive)
                        .Select(s => s.Id)
                        .FirstOrDefault(),

                    StoreName = c.StoreOwner.Stores
                        .Where(s => s.IsActive)
                        .Select(s => s.StoreName)
                        .FirstOrDefault(),

                    StoreLogo = c.StoreOwner.Stores
                        .Where(s => s.IsActive)
                        .Select(s => s.Logo)
                        .FirstOrDefault(),

                    Title = c.Title,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,

                    NumberOfUnReadMessage = c.Messages
                        .Count(e => !e.IsRead && e.SenderId != userId),
                    LastMessageAt = c.Messages
                .Select(m => (DateTime?)m.SentAt)
                .Max()


                })
                .AsNoTracking()
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();
        }

  /*      public async Task<List<ResponseGetComplaintByUserDto>> GetAllComplaintByUserAsync(string userId)
        {
            return await _dbContext.Complaints
                .Where(c => c.CustomerId == userId)
                .Select(c => new ResponseGetComplaintByUserDto
                {
                    ComplaintId = c.Id,
                    StoreId = c.StoreOwner.Stores
                        .Where(s => s.IsActive)
                        .Select(s => s.Id)
                        .FirstOrDefault(),

                    StoreName = c.StoreOwner.Stores
                        .Where(s => s.IsActive)
                        .Select(s => s.StoreName)
                        .FirstOrDefault(),

                    StoreLogo = c.StoreOwner.Stores
                        .Where(s => s.IsActive)
                        .Select(s => s.Logo)
                        .FirstOrDefault(),

                    Title = c.Title,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    NumberOfUnReadMessage = c.Messages.Count(e => !e.IsRead && e.SenderId != userId)
                })
                .AsNoTracking()
                .OrderByDescending(c=>c.CreatedAt)
                .ToListAsync();
        }
  */
        public async Task<Complaint> GetComplaintAsync(string userId, string storeOwnerId)
        {
            var res=await _dbContext.Complaints.FirstOrDefaultAsync(c=>c.CustomerId==userId && c.StoreOwnerId==storeOwnerId);
            return res;
        }
    }
}
