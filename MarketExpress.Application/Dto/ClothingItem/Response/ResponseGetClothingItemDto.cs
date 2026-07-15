using MarketExpress.Application.Dto.ProductSize.Response;
using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.ClothingItem.Response
{
    public class ResponseGetClothingItemDto
    {
        public int Id { get; set; }
     
        public string Color { get; set; }
        public string Image {  get; set; }
        
      
        public List<ResponseGetAllSizeDto> ProductSizes { get; set; }
    }
}
