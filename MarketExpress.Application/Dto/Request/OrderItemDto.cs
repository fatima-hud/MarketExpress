using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Request
{
   public class OrderItemDto
    {
        
        public int ProductId { get; set; }
        public int Quantity { get; set; }
       
        public string Size { get; set; }
        public string Color { get; set; }
    }
}
