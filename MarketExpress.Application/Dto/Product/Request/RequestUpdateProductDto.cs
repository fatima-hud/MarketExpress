using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Product.Request
{
   public class RequestUpdateProductDto
    {
       
       
        public decimal? Price { get; set; }
       
        public int? CategoryId { get; set; }
        public int? DiscountPrecentage {  get; set; }
        public IFormFile ?Image { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime ?DiscountEndDate { get; set; }
        public string? Name {  get; set; }
        public string? Description {  get; set; }
      
        
       
    }
}
