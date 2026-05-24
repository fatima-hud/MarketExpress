using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.ClothingItem.Request
{
  public  class RequestSuggestProductDto
    {
        public int ProductId { get; set; }
        public string Color { get; set; }
        public enSize Size { get; set; }
    }
}
