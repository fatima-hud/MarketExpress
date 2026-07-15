using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
       
        public int CategoryId { get; set; } 
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount =>
    (DiscountPercentage.HasValue &&
     DiscountStartDate.HasValue&&
     DiscountEndDate.HasValue&&
     DiscountStartDate <= DateTime.UtcNow &&
     DiscountEndDate >= DateTime.UtcNow)
        ? Price - (Price * (DiscountPercentage.Value / 100m))
        : Price;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]

        
        public enSeason Season {  get; set; }
        public enGender Gender {  get; set; }
        public enType Type { get; set; }
        public string Image { get; set; }
        public enOccasion Occasion { get; set; }




        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }
        public decimal ?DiscountPercentage { get; set; }
        public DateTime ?DiscountStartDate { get; set; }
        public DateTime ?DiscountEndDate { get; set; }


       
       
        public ICollection<OrderItem> OrderItems{ get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
