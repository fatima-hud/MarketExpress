using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Order.Response
{
   public class ResponseOrderItemDto
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity {  get; set; }
        public enSize Size {  get; set; }
        public string Color {  get; set; }
        public string ColorHex { get; set; }
        public decimal Price { get; set; }
      
        
        
    }
}
