using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Delivery.Request
{
    public class RequestAddDeliveryDto
    {
        public int OrderId { get; set; }
        public string DriverId { get; set; }
      
    }
}
