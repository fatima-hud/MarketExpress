using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Response
{
    public class ResponseCartItemDto
    {

      
        public int CartId { get; set; }
        public int StoreId {  get; set; }
        public int ProductId { get; set; }
        public string ProductImage {  get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount {  get; set; }
        public enSize Size { get; set; }
        public string Color { get; set; }
        public string ColorHexCode {  get; set; }

        
    }
}
