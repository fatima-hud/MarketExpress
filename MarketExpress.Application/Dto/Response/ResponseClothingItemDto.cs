using MarketExpress.Application.Dto.ProductSize.Response;
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
        public int Id {  get; set; }
        public int productId {  get; set; }
            public string Color { get; set; } 

        public string ColorHexCode {  get; set; }
        public string Image {  get; set; }
    
        
    }
}
