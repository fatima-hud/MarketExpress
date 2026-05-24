using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
  public  class TransactionProcess
    {
        public int Id { get; set; }
        public Guid WalletId { get; set; }
        public int? PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public  enTransactionType TransactionType { get; set; }
        public Wallet Wallet { get; set; }
        public Payment ?Payment { get; set; }

    }
}
