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

        public async Task<List<Product>> FilterAsync(decimal? minPrice, decimal? maxPrice, enType? Type, string? Color, enSize? Size)
        {
            IQueryable<Product> query = _dbContext.Products.Include(e => e.ClothingItems);


            if (minPrice.HasValue)
            {
                query = query.Where(e => e.PriceAfterDiscount >= minPrice);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(e => e.PriceAfterDiscount <= maxPrice);
            }

            if (Type.HasValue)
            {
                query = query.Where(e => e.Type == Type.Value);
            }
            if (!string.IsNullOrEmpty(Color))
            {
                query = query.Where(e => e.ClothingItems.Any(c => c.Color.ToLower() == Color.ToLower()));

            }
            if (Size.HasValue)
                query = query.Where(e => e.ClothingItems.Any(c => c.Size == Size.Value));







            return await query.ToListAsync();


        }

        public async Task<List<Product>> SearchAsync(string query)
        {
            var lowerQuery = query.ToLower();

          
            Enum.TryParse<enType>(query, true, out var productType);

          
            Enum.TryParse<enSize>(query, true, out var size);

            var products = await _dbContext.Products
                .Include(e => e.ClothingItems)
                .Where(e =>

                    e.Description.ToLower().Contains(lowerQuery) ||

                    e.Name.ToLower().Contains(lowerQuery) ||

                    e.Type == productType ||

                    e.ClothingItems.Any(o =>

                        o.Color.ToLower().Contains(lowerQuery) ||

                        o.Size == size
                    )
                )
                .ToListAsync();

            return products;
        }
        public async Task<bool>IsProductHasSizeAsync(int productId, enSize size)
        {
            var product = await _dbContext.Products.Include(e => e.ClothingItems).FirstOrDefaultAsync(e => e.Id == productId);
            if (product == null)
                return false;
            return product.ClothingItems.Any(c => c.Size == size);
        }
        public async Task<bool> IsProductHasColorAsync(int productId, string Color)
        {
            var product = await _dbContext.Products.Include(e => e.ClothingItems).FirstOrDefaultAsync(e => e.Id == productId);
            if (product == null)
                return false;
            return product.ClothingItems.Any(c => c.Color.ToLower()==Color.ToLower());
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
    }
}





      