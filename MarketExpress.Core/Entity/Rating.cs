using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class Rating
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        public int ProductId {  get; set; }
        public int? RatingValue {  get; set; }
      
        public DateTime CreatedAt { get; set; }
        public Product Product {  get; set; }
    }
}
