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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
           
            builder.Property(p => p.Price).HasPrecision(18, 2);



            builder
           .HasOne(oi => oi.order)
           .WithMany(o => o.Items)
           .HasForeignKey(oi => oi.OrderId)
           .OnDelete(DeleteBehavior.Cascade);


            builder
            .HasOne(oi => oi.product)
           .WithMany(o => o.OrderItems)
           .HasForeignKey(oi => oi.ProductId)
           .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
