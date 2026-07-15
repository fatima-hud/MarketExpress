using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepositoryAsync<Product>, IProductRepository
    {


        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {


        }

        public async Task<List<Product>> FilterAsync( enType? Type, string? Color, enSize? Size)
        {
            IQueryable<Product> query = _dbContext.Products.Include(e => e.ProductColors).ThenInclude(e=>e.ProductSizes);




            if (Type.HasValue)
            {
                query = query.Where(e => e.Type == Type.Value);
            }
            if (!string.IsNullOrEmpty(Color))
            {
                query = query.Where(e => e.ProductColors.Any(c => c.Color.ToLower() == Color.ToLower()));

            }
            if (Size.HasValue)
                query = query.Where(e => e.ProductColors.Any(c => c.ProductSizes.Any(e=>e.Size==Size.Value) ));

            return await query.ToListAsync();


        }

        public async Task<List<Product>> SearchAsync(string query)
        {
            var terms = query
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            IQueryable<Product> products = _dbContext.Products
                .Include(e=>e.ProductColors).ThenInclude(e => e.ProductSizes);

            // النوع
            var parsedType = terms
                .FirstOrDefault(t => Enum.TryParse<enType>(t, true, out _));

            if (parsedType != null)
            {
                Enum.TryParse(parsedType, true, out enType type);

                products = products.Where(p => p.Type == type);
            }

            // القياس
            var parsedSize = terms
                .FirstOrDefault(t => Enum.TryParse<enSize>(t, true, out _));

            if (parsedSize != null)
            {
                Enum.TryParse(parsedSize, true, out enSize size);

                products = products.Where(p =>
                    p.ProductColors.Any(e=>e.ProductSizes.Any(s=>s.Size == size)));
            }

            // اللون
            var colors = terms.Where(t =>
                !Enum.TryParse<enType>(t, true, out _) &&
                !Enum.TryParse<enSize>(t, true, out _));

            foreach (var color in colors)
            {
                products = products.Where(p =>
                    p.ProductColors.Any(c =>
                        c.Color.ToLower().Contains(color)

                        ||

                        p.Name.ToLower().Contains(color)

                        ||

                        p.Description.ToLower().Contains(color)
                    ));
            }

            return await products.ToListAsync();
        }
        public async Task<bool>IsProductHasSizeAsync(int productColorId, enSize size)
        {
            var product = await _dbContext.ProductColors.Include(e =>e.ProductSizes).FirstOrDefaultAsync(e => e.Id == productColorId);
            if (product == null)
                return false;
            return product.ProductSizes.Any(c => c.Size == size);
        }
        public async Task<bool> IsProductHasColorAsync(int productId, string Color)
        {
            var product = await _dbContext.Products.Include(e => e.ProductColors).FirstOrDefaultAsync(e => e.Id == productId);
            if (product == null)
                return false;
            return product.ProductColors.Any(c => c.Color.ToLower()==Color.ToLower());
        }

        public async Task<double> GetRatingAsync(int productId)
        {
            var ratings = await _dbContext.Ratings
        .Where(e => e.ProductId == productId && e.RatingValue.HasValue)
        .Select(e => e.RatingValue.Value)
        .ToListAsync();
            if (ratings.Count == 0)
                return 0;
            return ratings.Average();

        }

        public async Task<Store> GetStoreByProductIdAsync(int ProductId)
        {
            var res=await _dbContext.Products.Include(e => e.Store)
                .Where(e => e.Id == ProductId)
                .Select(e => e.Store)
                .FirstOrDefaultAsync();
            return res;
        }

        public async Task<List<Product>> GetAllProductByStoreAsync(int storeId)
        {
            var res = await _dbContext.Products.Where(e => e.StoreId == storeId).ToListAsync();
            return res;
        }

        public async Task<List<Product>> GetAllDiscountProductsByStoreAsync(int storeId)
        {
            var dayDate = DateTime.UtcNow;
             var res= await _dbContext.Products.Where(e => e.StoreId == storeId && e.DiscountPercentage>0 &&e.DiscountStartDate<=dayDate && e.DiscountEndDate>dayDate ).ToListAsync(); 
            return res;
        }

        public async Task<List<Product>> GetAllDiscountProductsByAllStoresAsync()
        {
            var dayDate = DateTime.UtcNow;
            var res = await _dbContext.Products.Where(e => e.DiscountPercentage > 0 && e.DiscountStartDate <= dayDate && e.DiscountEndDate > dayDate).ToListAsync();
            return res;
        }
    }
}





