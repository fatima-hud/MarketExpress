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


            /*   var products = new List<Product>
               {
                   new Product{Name="طقم رياضي",StoreId=4,Description="مجموعة رياضية بلوزة قصيرة الاكمام وشورت ",CategoryId=5,Gender=enGender.Male,Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936006/photo_2026-06-20_09-06-22_jrlfvi.jpg",Occasion=enOccasion.Sport,Price=3000,Season=enSeason.Summer,Type=enType.SportSet},
                    new Product{Name="طقم رياضي",StoreId=4,Description="مجموعة رياضية بلوزة قصيرة الاكمام و بنطال ",CategoryId=5,Gender=enGender.Male,Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936005/photo_2026-06-20_09-06-09_dzmkow.jpg",Occasion=enOccasion.Sport,Price=3000,Season=enSeason.Summer,Type=enType.SportSet},
                       new Product{Name="قميص رياضي",StoreId=4,Description="قميص رجالي صيفي خفيف الوزن قصير الاكمام  ",CategoryId=5,Gender=enGender.Male,Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936008/photo_2026-06-20_09-06-29_set7v0.jpg",Occasion=enOccasion.Sport,Price=3000,Season=enSeason.Summer,Type=enType.T_shirt}






               };
        await dbContext.Set<Product>().AddRangeAsync(products);
          await dbContext.SaveChangesAsync();*/
            var productColor = new List<ProductColor> {


                new ProductColor{ProductId=12,Color="اسود",ColorHexCode="#000000",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130690/58codv1v3lzc8qem3evsls33c_thpvuq.jpg"},

new ProductColor{ProductId=12,Color= "اخضر",ColorHexCode="#006B4A",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130690/5gwd2c616gyoiv90xtp9182uc_osaudy.jpg"},

new ProductColor{ProductId=13,Color="ابيض",ColorHexCode="#FFFFFF",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130691/IMG_%D9%A2%D9%A0%D9%A2%D9%A6%D9%A0%D9%A4%D9%A0%D9%A7_%D9%A1%D9%A9%D9%A1%D9%A5%D9%A3%D9%A2_qhpbo9.jpg"},

new ProductColor{ProductId=13,Color="اسود",ColorHexCode="#000000",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130691/IMG_%D9%A2%D9%A0%D9%A2%D9%A6%D9%A0%D9%A4%D9%A0%D9%A7_%D9%A1%D9%A9%D9%A1%D9%A5%D9%A1%D9%A4_yqjion.jpg"},

new ProductColor{ProductId=14,Color="اسود",ColorHexCode="#000000",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130691/2a8oujd9t9tgwgfxv5fxshb88_dxxb40.webp"},

new ProductColor{ProductId=14,Color="ابيض",ColorHexCode="#FFFFFF",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130692/2rug1r130gemhb6fbtbnp12wf_vlnpxr.webp"},


new ProductColor{ProductId=15,Color="اخضر",ColorHexCode="#134B36",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130691/6vhntdazvgq0ovptb7ywmvqsw_yuelkm.jpg"},

new ProductColor{ProductId=15,Color="ابيض ",ColorHexCode="#FFFFFF",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130692/272gcj3b89wu93enjd25tq771_cg9xur.webp"},


new ProductColor{ProductId=16,Color="احمر ",ColorHexCode="#B1002C",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130698/6xplagf4v5wuad68euopilnk3_dp0u9b.webp"},

new ProductColor{ProductId=16,Color="اسود",ColorHexCode="#000000",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130693/233v1mvi7i5tdmfc9wwofzvu0_sv3k9m.webp"},


new ProductColor{ProductId=17,Color="ابيض",ColorHexCode="#FFFFFF",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130693/5pesuge9cayrvxvpu8fw0fk74_a8twz3.webp"},


new ProductColor{ProductId=17,Color="اسود ",ColorHexCode="#000000",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130692/5hv0tcoi2x14vg15sj6aqhkrg_awbsi4.webp"},

new ProductColor{ProductId=18,Color="ازرق ",ColorHexCode="#385C80",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781136218/ae2nrsp7l6lvbdclv2jhq01s_uzldq3.webp"},

new ProductColor{ProductId=18,Color="ابيض",ColorHexCode="#FFFFFF",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781136218/1apmqhgtg8pwd36pfmpwgop2t_xbb085.webp"},


new ProductColor{ProductId=19,Color="ابيض",ColorHexCode="#FFFFFF",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130692/3abrc8uv76md2ny7kpyvpp4zz_etp3pb.webp"},

new ProductColor{ProductId=19,Color="اسود ",ColorHexCode="000000",Image="https://res.cloudinary.com/dg3xp6k1m/image/upload/q_auto/f_auto/v1781130692/3wp9ybzw823tfzlzxcisulace_qdg4e4.webp"},





             new ProductColor{ ProductId=26,Color="فضي",ColorHexCode="#C0C0C0",Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936006/photo_2026-06-20_09-06-22_jrlfvi.jpg"},
              new ProductColor{ ProductId=26,Color="اسود",ColorHexCode="#000000",Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936006/photo_2026-06-20_09-06-18_tityqd.jpg"},
                 new ProductColor{ ProductId=26,Color="ابيض",ColorHexCode="#FFFFFF",Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936005/photo_2026-06-20_09-06-14_zoerd4.jpg"},


             new ProductColor{ ProductId=27,Color="فضي",ColorHexCode="#C0C0C0",Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936005/photo_2026-06-20_09-06-06_orushg.jpg"},
              new ProductColor{ ProductId=27,Color="اسود",ColorHexCode="#000000",Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936005/photo_2026-06-20_09-05-29_dzbzzt.jpg"},
                 new ProductColor{ ProductId=27,Color="ابيض",ColorHexCode="#FFFFFF",Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936005/photo_2026-06-20_09-06-09_dzmkow.jpg"},

                 new ProductColor{ProductId=28,Color="ازرق",ColorHexCode="#385C80",Image="https://res.cloudinary.com/dosaekozq/image/upload/v1781936008/photo_2026-06-20_09-06-29_set7v0.jpg"}

               };
            //    await dbContext.Set<ProductColor>().AddRangeAsync(productColor);
            //      await dbContext.SaveChangesAsync();

            var productSizes = new List<ProductSize>
            {

                /*   new ProductSize{ProductColorId=36,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=36,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=36,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=37,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=37,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=37,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=38,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=38,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=38,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=39,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=39,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=39,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=40,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=40,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=40,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=41,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=41,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=41,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=42,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=42,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=42,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=43,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=43,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=43,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=44,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=44,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=44,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=45,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=45,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=45,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=46,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=46,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=46,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=47,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=47,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=47,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=48,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=48,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=48,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=49,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=49,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=49,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=50,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=50,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=50,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=51,Quantity=20,Size=enSize.M},
       new ProductSize{ProductColorId=51,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=51,Quantity=30,Size=enSize.XL}

          new ProductSize{ProductColorId=52,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=52,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=52,Quantity=20,Size=enSize.XXL},

        new ProductSize{ProductColorId=53,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=53,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=53,Quantity=20,Size=enSize.XXL},

        new ProductSize{ProductColorId=54,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=54,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=54,Quantity=20,Size=enSize.XXL},

        new ProductSize{ProductColorId=55,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=55,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=55,Quantity=20,Size=enSize.XXL},


       new ProductSize{ProductColorId=56,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=56,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=56,Quantity=20,Size=enSize.XXL},

       new ProductSize{ProductColorId=57,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=57,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=57,Quantity=20,Size=enSize.XXL},

       new ProductSize{ProductColorId=58,Quantity=15,Size=enSize.L},
       new ProductSize{ProductColorId=58,Quantity=30,Size=enSize.XL},
       new ProductSize{ProductColorId=58,Quantity=20,Size=enSize.XXL},










                };*/
                //   await dbContext.Set<ProductSize>().AddRangeAsync(productSizes);
                // await dbContext.SaveChangesAsync();


            };
        }
    }
}
