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
    public class ClothingItemConfiguration : IEntityTypeConfiguration<ClothingItem>
    {
        public void Configure(EntityTypeBuilder<ClothingItem> builder)
        {
            builder.HasIndex(c => new { c.ProductId,c.Size,c.Color }).IsUnique();
            builder.Property(e => e.Size).HasConversion<string>();
            builder.ToTable(t => t.HasCheckConstraint("CK_Product_Quantity", "[Quantity] >= 0"));
        }
    }
}
