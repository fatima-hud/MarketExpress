using MarketExpress.Core.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class Wallet
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public decimal Balance { get; set; }
     
        public ICollection<TransactionProcess> Transactions { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public User User { get; set; }

    }
}
