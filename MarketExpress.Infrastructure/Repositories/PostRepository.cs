using MarketExpress.Core.Entity;
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
    public class PostRepository : GenericRepositoryAsync<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }



        public async Task<List<Post>> GetAllPostsByStoreAsync(string? userId, int storeId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return await _dbContext.Posts
               .Include(p => p.PostMedias)
               .Include(p => p.PostReactions)
               .Where(p =>
                   p.StoreId == storeId &&
                   (
                       p.Visibility == enPostVisibility.Public
                      
                   )
               )
               .OrderByDescending(p => p.CreatedAt)
               .ToListAsync();
            }
               

            return await _dbContext.Posts
                .Include(p => p.PostMedias)
                .Include(p => p.PostReactions)
                .Where(p =>
                    p.StoreId == storeId &&
                    (
                        p.Visibility == enPostVisibility.Public
                        ||
                        (
                            p.Visibility == enPostVisibility.Followers &&
                            p.Store.StoreFollowers.Any(f =>
                                f.UserId == userId &&
                                f.IsFollow
                            )
                        )
                    )
                )
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
