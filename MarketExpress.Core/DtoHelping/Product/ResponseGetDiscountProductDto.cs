using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Product.Response
{
   public class ResponseGetDiscountProductDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPercentage { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public decimal? PriceAfterDiscount { get; set; }

        public string Image { get; set; }
    }
}
