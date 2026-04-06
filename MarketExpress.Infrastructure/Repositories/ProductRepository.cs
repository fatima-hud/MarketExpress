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

        public async Task<List<Product>> FilterAsync(decimal? minPrice, decimal? maxPrice, enType? Type, string? Color, string? Size)
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
               query=query.Where(e=>e.Type == Type.Value);
            }
            if (!string.IsNullOrEmpty(Color))
            {
                   query=query.Where(e => e.ClothingItems.Any(c=>c.Color.ToLower()==Color.ToLower()));
                
            }
            if (!string.IsNullOrEmpty(Size))
                query = query.Where(e => e.ClothingItems.Any(c => c.Size.ToLower() == Size.ToLower()));







            return await query.ToListAsync();


        }

    


       

        public async Task<List<Product>> SearchAsync(string query)
        {
           
            if (Enum.TryParse<enType>(query, true, out var productType))
            {
                var lowerQuery = query.ToLower();

                var products = await _dbContext.Products.Include(e=>e.ClothingItems)

                    .Where(e =>
                        e.Description.ToLower().Contains(lowerQuery) ||
                        e.Type == productType ||
                        e.Name.ToLower().Contains(lowerQuery) ||
                        e.ClothingItems.Any(
                            o => o.Color.ToLower().Contains(lowerQuery) ||
                        o.Size.ToLower().Equals(lowerQuery)
                        )).ToListAsync();



                   
                    
                

                return products;
            }
            else
            {
               
                var products = await _dbContext.Products
                    .Where(e => e.Description.ToLower().Contains(query.ToLower()))
                    .ToListAsync();

                return products;
            }
        }

      
    }
}
