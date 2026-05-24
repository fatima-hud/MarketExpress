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
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasOne(e=>e.Driver).WithMany(e=>e.Deliveries).HasForeignKey(e=>e.DriverId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Order).WithOne(e => e.Delivery).HasForeignKey<Delivery>(e => e.OrderId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
