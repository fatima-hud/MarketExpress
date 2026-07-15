using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.ProductSize.Request
{
    public class RequestProductSizeDto
    {
        public enSize Size { get; set; }
        public int Quantity { get; set; }

    }
}
