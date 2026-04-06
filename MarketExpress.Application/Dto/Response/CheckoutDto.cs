using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Response
{
   public class CheckoutDto
    {
        public OrderResponseDto Order { get; set; }
        public List<OrderItemResponseDto> Items { get; set; }

    }
}
