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
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
           

            builder
    .HasOne(x => x.Cart)
    .WithMany(x => x.CartItems)
    .HasForeignKey(x => x.CartId)
    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
