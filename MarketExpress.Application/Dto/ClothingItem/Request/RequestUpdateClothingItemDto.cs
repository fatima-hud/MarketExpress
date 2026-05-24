using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.ClothingItem.Request
{
   public  class RequestUpdateClothingItemDto
    {
        public enSize? Size {  get; set; }
        public string? Color { get; set; }
        public int? Quantity { get; set; }
       
    }
}
