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
        public enSize Size { get; set; }
        public int Quantity { get; set; }

        public bool IsFoundProduct { get; set; }
    }
}
