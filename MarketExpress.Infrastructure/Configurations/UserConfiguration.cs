using MarketExpress.Core.Entity.Identity;
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
   public class UserConfiguration:IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.Gender).HasConversion<string>();
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(e => e.PhoneNumber).HasMaxLength(10);
           
            /*     builder
          .HasOne(u => u.Store)
          .WithMany(s => s.Employees)
          .HasForeignKey(u => u.StoreId)
          .OnDelete(DeleteBehavior.Restrict); */
        }

       
    }
}
