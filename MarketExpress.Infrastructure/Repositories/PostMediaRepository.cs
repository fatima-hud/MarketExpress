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
    public class PostMediaRepository : GenericRepositoryAsync<PostMedia>, IPostMediaRepository
    {
        public PostMediaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<PostMedia>> GetAllMediasByPostAsync(int postId)
        {
            var res = await _dbContext.PostMedias.Where(e => e.PostId == postId).ToListAsync();
            return res;
        }
    }
}
