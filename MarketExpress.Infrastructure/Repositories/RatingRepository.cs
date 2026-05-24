using MarketExpress.Core.Entity;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class RatingRepository : GenericRepositoryAsync<Rating>, IRatingRepository
    {
        public RatingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Rating> GetUserRatingForProductAsync(string userId, int productId)
        {
          var res=await _dbContext.Ratings.FirstOrDefaultAsync(r =>r.UserId==userId && r.ProductId==productId );
            return res;
        }
    }
}
