using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.CartItem.Response
{
   public class ResponseGetInfoCartDto
    {
        public List<ResponseGetCartItemDto> CartItemDto {  get; set; }
        public decimal TotalPrice {  get; set; }
    }
}
