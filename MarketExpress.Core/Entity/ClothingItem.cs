using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class ClothingItem
    {
        public int Id { get; set; }
        public int ProductId {  get; set; }
        public string Color {  get; set; }
        public string Size {  get; set; }
        public bool IsFoundProduct {  get; set; }
        public Product Product { get; set; }


    }
}
