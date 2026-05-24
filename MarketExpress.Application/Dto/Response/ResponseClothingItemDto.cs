using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Response
{
   public  class ResponseClothingItemDto
    {
        public int productId {  get; set; }
            public enSize Size { get; set; }
            public string Color { get; set; }
            public int Quantity { get; set; }
            public bool IsFound { get; set; }
        public string ColorHexCode {  get; set; }

        
    }
}
