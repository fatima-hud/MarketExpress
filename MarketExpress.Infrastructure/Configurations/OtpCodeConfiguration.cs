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
    public class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
    {
        public void Configure(EntityTypeBuilder<OtpCode> builder)
        {
           
            builder
          .HasOne(o => o.User)
          .WithMany(o => o.OtpCodes)
          .HasForeignKey(o => o.UserId)
          .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
