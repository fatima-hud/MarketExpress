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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(e=>e.User).WithMany(e=>e.Comments).HasForeignKey(e=>e.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Product).WithMany(e => e.Comments).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(e => e.Content).IsRequired().HasMaxLength(500);
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
