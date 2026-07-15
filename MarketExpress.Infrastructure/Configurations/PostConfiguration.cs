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
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
           builder.HasMany(p => p.PostReactions)
                .WithOne(pr => pr.Post)
                .HasForeignKey(pr => pr.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.PostMedias)
                .WithOne(pm => pm.Post)
                .HasForeignKey(pm => pm.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Store)
                .WithMany(s => s.Posts)
                .HasForeignKey(p => p.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(p => p.Visibility)
                .HasConversion<string>();
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
