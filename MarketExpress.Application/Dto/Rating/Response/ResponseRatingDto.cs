using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Rating.Response
{
    public class ResponseRatingDto
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int RatingValue { get; set; }
      
        public DateTime CreatedAt { get; set; }
    }
}
