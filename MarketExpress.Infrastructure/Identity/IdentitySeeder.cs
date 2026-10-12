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
using static System.Net.WebRequestMethods;

namespace MarketExpress.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        // private static readonly ApplicationDbContext _dbContext;


        public static async Task SeedDataAsync(RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            string[] roles = { "Admin", "User", "SuperAdmin", "Driver", "EmployeeOfPayment", "Dispatcher", "SocialMediaManager" };

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
                  new Category { Name = "ملابس كاجوال نسائية" },
                  new Category { Name = "ملابس كاجوال رجالية" },
                  new Category { Name = "فساتين نسائية" },


                  new Category { Name = "ملابس رياضيةرجالية" },
                    new Category { Name = "احذية رجالية " }

                };

                await dbContext.Set<Category>().AddRangeAsync(categoryList);
                await dbContext.SaveChangesAsync();
            }








        }
    }
}
