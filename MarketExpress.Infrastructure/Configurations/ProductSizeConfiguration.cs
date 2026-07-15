using MarketExpress.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Configurations
{
    public class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
    {
        public void Configure(EntityTypeBuilder<ProductSize> builder)
        {
            builder.HasIndex(e => new { e.ProductColorId, e.Size }).IsUnique();
            builder.Property(e => e.Size).HasConversion<string>();
            builder.ToTable(t => t.HasCheckConstraint("CK_Product_Quantity", "[Quantity] >= 0"));
            builder.HasOne(e => e.ProductColor).WithMany(e => e.ProductSizes).HasForeignKey(e => e.ProductColorId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
