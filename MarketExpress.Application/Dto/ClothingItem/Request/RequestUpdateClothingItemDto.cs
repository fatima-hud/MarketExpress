using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.ClothingItem.Request
{
   public  class RequestUpdateClothingItemDto
    {
      
        public string? Color { get; set; }
        public IFormFile ?Image { get; set; }
     
       
    }
}
