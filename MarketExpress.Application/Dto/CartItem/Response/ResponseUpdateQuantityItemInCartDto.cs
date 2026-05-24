using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.CartItem.Response
{
    public class ResponseUpdateQuantityItemInCartDto
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }

    }
}
