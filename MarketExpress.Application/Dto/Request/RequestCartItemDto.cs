using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Request
{
    public class RequestCartItemDto
    {
      
        [Required]
        public int Quantity { get; set; }
       
        [Required]
        public int ProductSizeId { get; set; }
     

    }
}
