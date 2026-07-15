using MarketExpress.Application.Dto.ProductSize.Request;
using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.ClothingItem.Request
{
   public class RequestAddClothingItemDto
    {
        [Required]
        public string Color { get; set; }
        [Required]
        public string ColorHexCode { get; set; }
        [Required]
        public IFormFile Image { get; set; }
       
        

    }
}
