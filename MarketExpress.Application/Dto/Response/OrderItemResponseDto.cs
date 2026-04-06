using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Response
{
   public class OrderItemResponseDto
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity {  get; set; }
        public string Size {  get; set; }
        public string Color {  get; set; }
      
        
        
    }
}
