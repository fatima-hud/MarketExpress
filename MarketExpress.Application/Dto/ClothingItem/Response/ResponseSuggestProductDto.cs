using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.ClothingItem.Response
{
   public class ResponseSuggestProductDto
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string? Name { get; set; } 
        public string Color { get; set; }
        public enSize Size { get; set; }
        public enGender Gender { get; set; }
        public enSeason Season { get; set; }
        public enOccasion Occasion { get; set; }
        public enType Type { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceAfterDiscount { get; set; }
        public int? RatingValue { get; set; }

    }
}
