using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.ClothingItem.Response
{
    public class ResponseGetAllColorDto
    {
        public string Color {  get; set; }
         public string ColorHexCode { get; set; }
        public bool IsFoundProduct { get; set; }
    }
}
