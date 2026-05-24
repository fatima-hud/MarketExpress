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
    public class DeliveryLocationConfiguration : IEntityTypeConfiguration<DeliveryLocation>
    {
        public void Configure(EntityTypeBuilder<DeliveryLocation> builder)
        {
            builder.HasOne(e=>e.Delivery).WithMany(e=>e.DeliveryLocations).HasForeignKey(e=>e.DeliveryId).OnDelete(DeleteBehavior.Cascade);    
        }
    }
}
