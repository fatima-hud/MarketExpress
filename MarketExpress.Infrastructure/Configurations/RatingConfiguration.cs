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
    public class RatingConfiguration:IEntityTypeConfiguration<Rating>
    {
       

        public void Configure(EntityTypeBuilder<Rating> builder)
        {
           
            builder.Property(e => e.Comment).HasMaxLength(100);
        }
    }
}
