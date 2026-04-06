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
    public class MessageRepository : GenericRepositoryAsync<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsMessageFoundByComplaintAsync(int complaintId, int messageId)
        {
            var ret=await _dbContext.Messages.AnyAsync(m => m.Id == messageId && m.ComplaintId == complaintId);
            return ret;
        }

        public async Task<List<Message>> GetAllMessageUnReadAsync(string userId, int complaintId)
        {
            var res=await _dbContext.Messages.Where(e=>e.ComplaintId == complaintId && e.SenderId != userId && !e.IsRead).ToListAsync();
            return res;
        }

        public async Task<List<Message>>?GetAllMessageByComplaintIdAsync(int complaintId)
        {
            var res = await _dbContext.Messages.Include(e=>e.Sender).Where(e => e.ComplaintId == complaintId).ToListAsync();
            return res;
        }
    }
}
