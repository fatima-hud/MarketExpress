using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.ProductInventory
{
  public  class ResponseGetOrderDetailesDto
    {
        public int ProductId { get; set; }
        public string Image { get; set; }
        public int NumberOfSales { get; set; }
        public decimal Price { get; set; }
        public int RemainingQuantity { get; set; }
        public enSize Size { get; set; }
        public string Color { get; set; }
        public string ColorHexCode { get; set; }
    }
}
