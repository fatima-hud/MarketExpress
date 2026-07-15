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
    public class ProductSizeRepository : GenericRepositoryAsync<ProductSize>, IProductSizeRepository
    {
        public ProductSizeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ProductSize> GetProductSizeAsync(int productId, string color, enSize size)
        {
          var res= await _dbContext.ProductColors.Where(e=>e.ProductId==productId && e.Color == color).SelectMany(e => e.ProductSizes.Where(f => f.Size == size)).FirstOrDefaultAsync();
            return res;
        }

        public async Task<ProductSize> GetProductSizeByProductColorAndSizeAsync(int productColorId, enSize size)
        {
        var res= await _dbContext.ProductSizes.FirstOrDefaultAsync(e => e.ProductColorId == productColorId && e.Size == size);
            return res;
        }
    }
}
