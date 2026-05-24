using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Payment.Response
{
    public class ResponseGetPaymentDto
    {
        public int Id { get; set; }
        public Guid WalletId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public enPaymentStatus Status { get; set; } 
        
    }
}
