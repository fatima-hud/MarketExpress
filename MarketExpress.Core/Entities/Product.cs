using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entities
{
   public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterDiscount {  get; set; }
        public int Quantity {  get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating {  get; set; }
        public EnSeason Season {  get; set; }
        public EnGender Gender {  get; set; }
        public EnType Type { get; set; }
        public byte[] Image { get; set; }
        public int StoreCategoryId {  get; set; }
        public StoreCategory StoreCategory { get; set; }
        public int DiscountSettingId {  get; set; }
        public DiscountSetting DiscountSetting { get; set; }

    }
}
