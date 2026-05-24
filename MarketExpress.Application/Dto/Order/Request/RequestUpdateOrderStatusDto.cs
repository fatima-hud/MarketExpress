using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Order.Request
{
   public class RequestUpdateOrderStatusDto
    {
        public enOrderStatus status {  get; set; }
    }
}
