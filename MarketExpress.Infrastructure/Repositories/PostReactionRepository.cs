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
    public class PostReactionRepository : GenericRepositoryAsync<PostReaction>, IPostReactionRepository
    {
        public PostReactionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PostReaction?> GetPostReactionByUserIdAndPostIdAsync(string userId, int postId)
        {
           var postReaction= await _dbContext.PostReactions.FirstOrDefaultAsync(pr => pr.UserId == userId && pr.PostId == postId);
            return postReaction;
        }

        public async Task<int> GetPostReactionsCountByPostIdAndReactionTypeAsync(int postId, enReactionType reactionType)
        {
            var count = await _dbContext.PostReactions.CountAsync(pr => pr.PostId == postId && pr.ReactionType == reactionType && pr.IsReacted);
            return count;
        }
    }
}
