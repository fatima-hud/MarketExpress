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
            public string Size { get; set; }
            public string Color { get; set; }
            public bool IsFound { get; set; }

        
    }
}
