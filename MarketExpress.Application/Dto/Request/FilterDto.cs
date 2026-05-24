using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Request
{
   public class FilterDto
    {
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }

        public enType? Type { get; set; }
        public string? Color { get; set; }
        public enSize? Size { get; set; }
    }
}
