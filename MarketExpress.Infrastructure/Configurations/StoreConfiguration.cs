using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Configurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
         
            builder.Property(e => e.StoreName).HasMaxLength(50);
            builder.Property(e => e.Description).HasMaxLength(100);
            builder.Property(s => s.Latitude).HasPrecision(9, 6);
            builder.Property(s => s.Longitude).HasPrecision(9, 6);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
       
            builder.Property(e => e.StoreStatus).HasDefaultValue(enStoreStatus.Pending);
            builder.HasIndex(s => s.OwnerId).IsUnique();


           
            builder.HasMany(e => e.Products).WithOne(e => e.Store).HasForeignKey(e => e.StoreId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e=>e.Owner).WithOne().HasForeignKey<Store>(e=>e.OwnerId).OnDelete(DeleteBehavior.Restrict);

       

        }
    }
}
