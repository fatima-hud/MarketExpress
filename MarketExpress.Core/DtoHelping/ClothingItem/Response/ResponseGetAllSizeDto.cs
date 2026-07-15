using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.ClothingItem.Response
{
   public class ResponseGetAllSizeDto
    {
        public int ProductSizeId { get; set; }
        public enSize Size { get; set; }
        public bool IsFoundProduct { get; set; }
    }
}
