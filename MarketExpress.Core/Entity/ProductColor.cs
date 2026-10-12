using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class ProductColor
    {
        public int Id { get; set; }
        public int ProductId {  get; set; }
        public string Color {  get; set; }
        public string ColorHexCode { get; set; }
       
        public string Image {  get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
        public Product Product { get; set; }


    }
}
