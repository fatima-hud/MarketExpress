using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public async Task<bool> CheckProductIsFound(int productId, string Color, enSize Size)
        {
            var prod=await _dbContext.Clothingitems.FirstOrDefaultAsync(e=>e.ProductId==productId&& e.Color==Color&&e.Size==Size);
            return prod!=null;
        }

        public async Task<List<ClothingItem>> GetAllSuggestClothingItemsByProductId(int productId,enSize Size,string Color)
        {
            var prod = await _dbContext.Products.FirstOrDefaultAsync(e => e.Id == productId);
            var res=  await _dbContext.Clothingitems.Include(e=>e.Product).Where(e => e.ProductId != productId && e.Size == Size && e.Color == Color&&e.Product.Gender==prod.Gender&&e.Product.Season==prod.Season&&e.Product.Occasion==prod.Occasion&&e.IsFoundProduct).ToListAsync();
            return res;
        }

        public async Task<List<ResponseGetAllColorDto>> GetAllColorByProduct(int productId)
        {


            var colors = await _dbContext.Clothingitems.Where(e => e.ProductId == productId)
                .GroupBy(e=>new { e.Color,e.ColorHexCode})
                .Select(c => new ResponseGetAllColorDto {
                Color = c.Key.Color,
                ColorHexCode = c.Key.ColorHexCode,
                IsFoundProduct = c.Any(x => x.Quantity > 0)

                }).Distinct().OrderBy(e=>e.Color).ToListAsync();
       
           
            return colors;
        }

        public async Task<List<ResponseGetAllSizeDto>> GetAllSizeByProduct(int productId)
        {
            var sizes = await _dbContext.Clothingitems.Where(e => e.ProductId == productId)
                .GroupBy(e=>e.Size)
                .Select(s=>new ResponseGetAllSizeDto
                {
                    size = s.Key,
                    IsFoundProduct = s.Any(x => x.Quantity > 0)
                }

                ) .Distinct().OrderBy(e=>e.size).ToListAsync();
               
           
            return sizes;
        }
        public async Task<ClothingItem>GetClothingItemByProductAsync(int productId, enSize Size, string Color)
        {
            var res=await _dbContext.Clothingitems.FirstOrDefaultAsync(e=>e.ProductId==productId&&e.Size==Size&&e.Color==Color);
            return res;
        }
        public async Task<List<ClothingItem>>GetAllClothingItemByProductAsync(int productId)
        {
            var res = await _dbContext.Clothingitems.Include(e => e.Product).Where(e => e.ProductId == productId).ToListAsync();
            return res;
        }

        public async Task<List<ResponseGetAllSizeDto>> GetAllSizeByProductColor(int productId, string color)
        {
            var res = await _dbContext.Clothingitems.Where(e => e.ProductId == productId && e.Color == color).Select(e => new ResponseGetAllSizeDto {
              size= e.Size,
              IsFoundProduct= e.IsFoundProduct
           }).Distinct().ToListAsync();
            return res;
        }

        public async Task<List<ResponseGetAllColorDto>> GetAllColorByProductSize(int productId, enSize size)
        {
            var res = await _dbContext.Clothingitems.Where(e => e.ProductId == productId && e.Size==size).Select(e => new ResponseGetAllColorDto { 
             Color=   e.Color ,
             ColorHexCode=e.ColorHexCode,
             IsFoundProduct=  e.IsFoundProduct
            }
            ).Distinct().ToListAsync();
            return res;
        }
    }
}
