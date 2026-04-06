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
    public class ClothingItemRepository : GenericRepositoryAsync<ClothingItem>, IClothingItemRepository
    {
        public ClothingItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckProductIsFound(int productId, string Color, string Size)
        {
            var prod=await _dbContext.Clothingitems.FirstOrDefaultAsync(e=>e.ProductId==productId&& e.Color==Color&&e.Size==Size);
            return prod!=null;
        }

        public async Task<List<string>> GetColorByProduct(int productId)
        {
           
           var items=await _dbContext.Clothingitems.Where(e=>e.ProductId==productId).ToListAsync();
          var colors=new List<string>();
            foreach (var item in items)
            {
                colors.Add(item.Color);
            }
            return colors;
        }

        public async Task<List<string>> GetSizeByProduct(int productId)
        {
            var items = await _dbContext.Clothingitems.Where(e => e.ProductId == productId).ToListAsync();
            var sizes = new List<string>();
            foreach (var item in items)
            {
                sizes.Add(item.Size);
            }
            return sizes;
        }
    }
}
