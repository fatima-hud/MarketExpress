using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Product.Request
{
   public class ProductRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]

        public enSeason Season { get; set; }
        [Required]
        public enGender Gender { get; set; }
        [Required]
        public enType Type { get; set; }
        [Required]
      
        public IFormFile Image { get; set; }
        [Required]
        public int StoreId {  get; set; }
        [Required]
        public int CategoryId {  get; set; }
        public decimal ?DiscountPrecentage {  get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        
    }
}

