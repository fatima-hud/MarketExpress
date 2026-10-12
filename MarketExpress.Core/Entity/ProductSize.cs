using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
    public class ProductSize
    {
        public int Id { get; set; }
        public int ProductColorId { get; set; }
        public enSize Size { get; set; }
        public int Quantity { get; set; }
        public bool IsFoundProduct => Quantity > 0;
        public bool IsDeleted { get; set; }
        public ProductColor ProductColor { get; set; }
      public ICollection<CartItem> CartItems { get; set; }

    }
}
