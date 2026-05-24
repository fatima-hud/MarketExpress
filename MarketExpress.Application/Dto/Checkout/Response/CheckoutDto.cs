using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Application.Dto.Payment.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Checkout.Response
{
   public class CheckoutDto
    {
        public ResponseAddOrderDto Order { get; set; }
        public List<ResponseOrderItemDto> Items { get; set; }
        public ResponseGetPaymentDto Payment {  get; set; }

    }
}
