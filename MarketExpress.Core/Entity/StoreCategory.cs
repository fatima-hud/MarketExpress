using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class StoreCategory
    {
        public int Id {  get; set; }
        public int StoreId {  get; set; }
        public int CategoryId { get; set; } 
        public Store Store {  get; set; }
        public Category Category {  get; set; }
        
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted {  get; set; }
        public DateTime DeletedAt { get; set; }

    }
}
