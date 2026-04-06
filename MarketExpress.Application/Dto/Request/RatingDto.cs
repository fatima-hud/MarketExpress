using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Request
{
   public class RatingDto
    {
        public int ProductId {  get; set; }
       
        public int? RatingValue {  get; set; }
        public string? Comment { get; set; }
    }
}
