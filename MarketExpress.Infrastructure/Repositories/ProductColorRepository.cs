using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class ProductColorRepository : GenericRepositoryAsync<ProductColor>, IProductColorRepository
    {
        public ProductColorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public Dictionary<MatchRule, List<MatchRule>> OutfitMatching =
        new Dictionary<MatchRule, List<MatchRule>>
    {
        {
            new MatchRule(enType.Shirt, enOccasion.Casual),

            new List<MatchRule>
            {
                new MatchRule(enType.Pants, enOccasion.Casual),
                new MatchRule(enType.Shoes, enOccasion.Sport),
                new MatchRule(enType.ShortPants, enOccasion.Casual)
            }
        },
         {
            new MatchRule(enType.T_shirt, enOccasion.Casual),

            new List<MatchRule>
            {
                new MatchRule(enType.Pants, enOccasion.Casual),
                new MatchRule(enType.Shoes, enOccasion.Sport),
                new MatchRule(enType.ShortPants, enOccasion.Casual)
            }
        },
          {
            new MatchRule(enType.Pants, enOccasion.Casual),

            new List<MatchRule>
            {
                new MatchRule(enType.Shirt, enOccasion.Casual),
                new MatchRule(enType.Shoes, enOccasion.Sport),
                new MatchRule(enType.T_shirt, enOccasion.Casual)
            }
        },
           {
            new MatchRule(enType.Shirt, enOccasion.Sport),

            new List<MatchRule>
            {
                new MatchRule(enType.Pants, enOccasion.Casual),
                new MatchRule(enType.Shoes, enOccasion.Sport),
             
            }
        },
             {
            new MatchRule(enType.T_shirt, enOccasion.Sport),

            new List<MatchRule>
            {
                new MatchRule(enType.Pants, enOccasion.Casual),
                new MatchRule(enType.Shoes, enOccasion.Sport),

            }
        },
          {
            new MatchRule(enType.ShortPants, enOccasion.Casual),

            new List<MatchRule>
            {
                new MatchRule(enType.Shirt, enOccasion.Casual),
                new MatchRule(enType.Shoes, enOccasion.Sport),
                new MatchRule(enType.T_shirt, enOccasion.Casual)
            }
        },



        {
            new MatchRule(enType.Dress, enOccasion.Party),

            new List<MatchRule>
            {
                new MatchRule(enType.Shoes, enOccasion.Formal),
                new MatchRule(enType.Shoes, enOccasion.Party),

            }
        },
         {
            new MatchRule(enType.Skirt, enOccasion.Party),

            new List<MatchRule>
            {
                new MatchRule(enType.Shirt, enOccasion.Party),
                new MatchRule(enType.Shoes, enOccasion.Formal),
                new MatchRule(enType.Shoes, enOccasion.Party),

            }
        },
          {
            new MatchRule(enType.Shoes, enOccasion.Party),

            new List<MatchRule>
            {
                new MatchRule(enType.Shirt, enOccasion.Party),
                new MatchRule(enType.Skirt, enOccasion.Party),
                new MatchRule(enType.Dress, enOccasion.Party),

            }
        },
           {
            new MatchRule(enType.Shoes, enOccasion.Sport),

            new List<MatchRule>
            {
                new MatchRule(enType.Shirt, enOccasion.Casual),
                new MatchRule(enType.T_shirt, enOccasion.Casual),
                new MatchRule(enType.Pants, enOccasion.Casual),
                 new MatchRule(enType.ShortPants, enOccasion.Casual),
                  new MatchRule(enType.SportSet, enOccasion.Sport),

            }
        },
            {
            new MatchRule(enType.SportSet, enOccasion.Sport),

            new List<MatchRule>
            {
              
                  new MatchRule(enType.Shoes, enOccasion.Sport)

            }
        }
    };

        public async Task<bool> CheckProductIsFound(int productId, string Color, enSize size)
        {
            var prod=await _dbContext.ProductColors.FirstOrDefaultAsync(e=>e.ProductId==productId&& e.Color==Color&&e.ProductSizes.Any(f=>f.Size==size));
            return prod!=null;
        }

        public async Task<List<ResponseSuggestProductDto>> GetAllSuggestClothingItemsByProductId(int productId)
        {
            var prod = await _dbContext.Products.FirstOrDefaultAsync(e => e.Id == productId);
               

            if (prod == null)
                return new List<ResponseSuggestProductDto>();


            var match = OutfitMatching.FirstOrDefault(e =>
                e.Key.Type == prod.Type &&
                e.Key.Occasion == prod.Occasion);

            if (match.Key == null || match.Value == null)
                return new List<ResponseSuggestProductDto>();


            var allowedMatches = match.Value;


            var query = _dbContext.ProductColors
      .Include(e => e.Product)
      .ThenInclude(e => e.Ratings)
      .Include(e => e.ProductSizes)
      .Where(e =>
          e.ProductId != productId &&
          e.ProductSizes.Any(s =>
              s.Quantity > 0) &&
          e.Product.Gender == prod.Gender &&
          e.Product.Season == prod.Season
      ).GroupBy(e => e.ProductId)
    .Select(g => g.First());
          
            var res = (await query.ToListAsync())
    .Where(e =>
        allowedMatches.Any(m =>
            m.Type == e.Product.Type &&
            m.Occasion == e.Product.Occasion))
    .Select(e => new ResponseSuggestProductDto
    {
        ProductId = e.ProductId,
        ImageUrl = e.Product.Image,
        Name = e.Product.Name,
        Color = e.Color,
        Gender = e.Product.Gender,
        Type=e.Product.Type,
        Season = e.Product.Season,
        Occasion = e.Product.Occasion,
        Price = e.Product.Price,

        PriceAfterDiscount =
            e.Product.Price -

            (
                e.Product.Price *
                (e.Product.DiscountPercentage ?? 0) / 100m
            ), 
      
        RatingValue =
    e.Product.Ratings.Any(r => r.RatingValue.HasValue)
        ? e.Product.Ratings
            .Where(r => r.RatingValue.HasValue)
            .Average(r => r.RatingValue!.Value)
        : 0
    })
        .ToList();
           
            return res;
        }

        public async Task<List<ResponseGetAllColorDto>> GetAllColorByProduct(int productId)
        {


            var colors = await _dbContext.ProductColors.Where(e => e.ProductId == productId)
                .GroupBy(e=>new { e.Color,e.ColorHexCode})
                .Select(c => new ResponseGetAllColorDto {
                Color = c.Key.Color,
                ColorHexCode = c.Key.ColorHexCode,
                IsFoundProduct = c.Any(x => x.ProductSizes.Any(e=>e.Quantity> 0))

                }).Distinct().OrderBy(e=>e.Color).ToListAsync();
       
           
            return colors;
        }

        public async Task<List<ResponseGetAllSizeDto>> GetAllSizeByProduct(int productId)
        {
           
               var res = await _dbContext.ProductColors.Where(e => e.ProductId == productId)
                .SelectMany(e => e.ProductSizes.Select(s => new ResponseGetAllSizeDto
                {
                    ProductSizeId=s.Id,
                    Size = s.Size,
                    IsFoundProduct = s.Quantity > 0
                }))
                .Distinct()
                .OrderBy(e => e.Size)
                .ToListAsync();
            return res;
        }
        public async Task<bool>IsProductColorHasSizeAsync(int productId, enSize Size, string Color)
        {
            var res=await _dbContext.ProductColors.AnyAsync(e=>e.ProductId==productId&&e.ProductSizes.Any(e=>e.Size==Size)&&e.Color==Color);
            return res;
        }
        public async Task<List<ProductColor>>GetAllClothingItemByProductAsync(int productId)
        {
            var res = await _dbContext.ProductColors.Include(e=>e.ProductSizes).Where(e => e.ProductId == productId).ToListAsync();
            return res;
        }

        public async Task<List<ResponseGetAllSizeDto>> GetAllSizeByProductColor(int productColorId)
        {
            var res = await _dbContext.ProductSizes.Where(e => e.ProductColorId == productColorId ).Select(e => new ResponseGetAllSizeDto {
                ProductSizeId=e.Id,
              Size= e.Size,
              IsFoundProduct= e.Quantity > 0
           }).Distinct().ToListAsync();
            return res;
        }
        public async Task<int> GetProductByProductColorAsync(int productColorId)
        {
            var ret = await _dbContext.ProductColors.FirstOrDefaultAsync(e => e.Id == productColorId);
            return ret.ProductId;
        }



        public async Task<List<ProductSize>> GetProductInventoryAlertAsync(int storeId)
        {
            var res = await _dbContext.ProductSizes.Include(e => e.ProductColor).ThenInclude(e=>e.Product).Where(e => e.ProductColor.Product.StoreId == storeId && e.Quantity<=5).ToListAsync();
            return res;
        }

        public async Task<string> GetClothingItemImageAsync(int productId,string color)
        {

            var res = await _dbContext.ProductColors.FirstOrDefaultAsync(e=>e.ProductId==productId && e.Color==color);
            return res.Image;

        }

        public async Task<ProductColor> GetProductColorIdAsync(int productId, string color)
        {
           var res=await _dbContext.ProductColors.FirstOrDefaultAsync(p=>p.ProductId==productId && p.Color==color);
            return res;
        }

        public async Task<bool> IsProductHasColorAsync(int productId,string color)
        {
            var res = await _dbContext.ProductColors.AnyAsync(e => e.ProductId == productId && e.Color == color);
            return res;
        }
        public async Task<ProductColor>GetProductColorWithProductAsync(int productColorId)
        {
            var res = await _dbContext.ProductColors.Include(e => e.Product).FirstOrDefaultAsync(e => e.Id == productColorId);
            return res;
        } 
    }
}
