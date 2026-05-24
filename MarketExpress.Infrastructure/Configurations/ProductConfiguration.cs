using MarketExpress.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
        

            builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(100);
            builder.Property(e => e.Image).HasMaxLength(500);
            builder.Property(p => p.DiscountPercentage).HasColumnType("decimal(5,2)");
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.Type).HasConversion<string>();
            builder.Property(p => p.Gender).HasConversion<string>();
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(p => p.Season).HasConversion<string>();
            builder.HasMany(e => e.Ratings).WithOne(e => e.Product).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Restrict);

       

            builder.ToTable(t => {
                t.HasCheckConstraint("CK_Product_Price", "[Price] > 0");
           
                t.HasCheckConstraint("CK_Product_Discount", "[DiscountPercentage] Is null or ([DiscountPercentage] >= 0 and [DiscountPercentage] <=100)");
                }
            );


        }
    }
}
