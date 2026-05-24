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
        public enSize size { get; set; }
        public bool IsFoundProduct { get; set; }
    }
}
