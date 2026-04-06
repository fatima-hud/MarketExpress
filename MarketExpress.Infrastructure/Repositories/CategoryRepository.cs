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
    public class CategoryRepository :  GenericRepositoryAsync<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

      
        public async Task<bool> IsCategoryFound(string Name)
        {
            var res= await _dbContext.Categories.FirstOrDefaultAsync(e=>e.Name==Name);
            return res != null;
            
        }
    }
}
