using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        // private static readonly ApplicationDbContext _dbContext;


        public static async Task SeedDataAsync(RoleManager<IdentityRole> roleManager,ApplicationDbContext dbContext)
        {
            string[] roles = { "Admin", "User", "SuperAdmin", "Driver", "EmployeeOfPayment", "Dispatcher" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            var categories = await dbContext.Set<Category>().ToListAsync();
            if (!categories.Any())
            {
                var categoryList = new List<Category>
                {
                  new Category { Name = "أحذية نسائية" },
    new Category { Name = "فساتين نسائية" }, 
    new Category { Name = "ملابس رياضية رجالية" },
    new Category { Name = "ملابس كاجوال نسائية" },

                };

                await dbContext.Set<Category>().AddRangeAsync(categoryList);
                await dbContext.SaveChangesAsync();
            }
        
            /*
            var products= new List<Product>
            {
                new Product {Name="",Description="",StoreId=,CategoryId=,Price=100,DiscountPercentage=null,Season=enSeason.Winter,Gender=enGender.Male,Type=enType.Skirt,Image="",Occasion=enOccasion.Casual},
                new Product { Name = "Product 2", Price = 20.0m },
                new Product { Name = "Product 3", Price = 30.0m }
            };*/
        }
    }
}
