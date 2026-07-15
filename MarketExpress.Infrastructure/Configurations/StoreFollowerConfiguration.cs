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
    public class StoreFollowerConfiguration : IEntityTypeConfiguration<StoreFollower>
    {
        public void Configure(EntityTypeBuilder<StoreFollower> builder)
        {
            builder
     .HasIndex(pr => new { pr.StoreId, pr.UserId })
     .IsUnique();
            builder.HasOne(sf => sf.User)
                .WithMany(u => u.StoreFollowers)
                .HasForeignKey(sf => sf.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e=>e.Store)
                .WithMany(s => s.StoreFollowers)
                .HasForeignKey(sf => sf.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
