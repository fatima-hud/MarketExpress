using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Transaction.Request
{
   public class RequestAddTransactionDto
    {
        public Guid WalletId { get; set; }
        public decimal Amount {  get; set; }
    }
}
