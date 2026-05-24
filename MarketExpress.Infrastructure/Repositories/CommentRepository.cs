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
    public class CommentRepository : GenericRepositoryAsync<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Comment>> GetAllCommentByProduct(int productId, int pageNumber, int pageSize)
        {
          var res=await _dbContext.Comments.Include(e=>e.User).Where(c => c.ProductId == productId).OrderByDescending(c => c.CreatedAt).Skip(pageSize*(pageNumber-1)).Take(pageSize).ToListAsync();
            return res;
        }
    }
}
