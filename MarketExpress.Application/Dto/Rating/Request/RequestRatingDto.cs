using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Rating.Request
{
   public class RequestRatingDto
    {
        [Required]
        public int ProductId {  get; set; }
     
        public int? RatingValue {  get; set; }
       
    }
}
