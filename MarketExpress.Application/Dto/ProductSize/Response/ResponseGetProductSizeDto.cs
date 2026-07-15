using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.ProductSize.Response
{
   public class ResponseGetProductSizeDto
    {
        public enSize Size { get; set; }
        public int Quantity { get; set; }
        public bool IsFound { get; set; }
    }
}
