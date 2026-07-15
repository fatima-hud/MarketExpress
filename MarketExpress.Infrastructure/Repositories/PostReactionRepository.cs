using MarketExpress.Core.Entity;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
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
    }
}
