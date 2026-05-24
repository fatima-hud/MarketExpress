using MarketExpress.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketExpress.Infrastructure.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(n => n.Type).HasConversion<string>();

            builder.HasMany(e => e.UserNotifications).WithOne(e => e.Notification).HasForeignKey(e => e.NotificationId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.CreatedBy).WithMany(e => e.AddNotifications).HasForeignKey(e => e.CreatedById).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
