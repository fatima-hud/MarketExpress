using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IPostReactionRepository:IGenericRepositoryAsync<PostReaction>
    {
        Task<PostReaction?> GetPostReactionByUserIdAndPostIdAsync(string userId, int postId);
        Task<int> GetPostReactionsCountByPostIdAndReactionTypeAsync(int postId, enReactionType reactionType);
    }
}
