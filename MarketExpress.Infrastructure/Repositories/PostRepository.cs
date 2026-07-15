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
    public class PostRepository : GenericRepositoryAsync<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Post>> GetAllPostsByStoreAsync(int storeId)
        {
            var ret = await _dbContext.Posts.Include(e=>e.PostMedias).Where(p => p.StoreId == storeId).OrderByDescending(p => p.CreatedAt).ToListAsync();
            return ret;
        }
    }
}
