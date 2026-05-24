using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.CartItem.Request
{
    public class RequestDeleteItemsFromCartDto
    {  
        public List<int>cartItemIds { get; set; }
    }
}
