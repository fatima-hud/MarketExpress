using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Product.Response
{
    public class ResponseProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int StoreId { get; set; }
        public int CategoryId { get; set; }
        public double Rating { get; set; }

        public decimal? DiscountPercentage { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public decimal? PriceAfterDiscount { get; set; }

        public string Image { get; set; }
        public List<ResponseGetAllSizeDto>? Sizes { get; set; }

        public List<ResponseGetAllColorDto>? Colors {  get; set; }



    }
}
