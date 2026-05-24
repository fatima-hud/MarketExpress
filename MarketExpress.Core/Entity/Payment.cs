using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class Payment
    {
        public int Id { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Wallet Wallet { get; set; }
        public ICollection<TransactionProcess> Transactions { get; set; }
        public enPaymentStatus Status { get; set; }

    }
}
