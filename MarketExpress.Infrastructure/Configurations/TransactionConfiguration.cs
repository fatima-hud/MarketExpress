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
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionProcess>
    {
        public void Configure(EntityTypeBuilder<TransactionProcess> builder)
        {
builder.HasOne(e=>e.Wallet).WithMany(e=>e.Transactions).HasForeignKey(e => e.WalletId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Payment).WithMany(e=>e.Transactions).HasForeignKey(e => e.PaymentId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.TransactionType).HasConversion<string>();
        }
    }
}
