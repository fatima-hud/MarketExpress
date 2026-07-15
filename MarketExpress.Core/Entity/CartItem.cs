using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId {  get; set; }
       
        public int ProductSizeId {  get; set; }
        public int Quantity {  get; set; }
       
        
        public Cart Cart { get; set; }
      
        public ProductSize ProductSize { get; set; }

        
    }
}
